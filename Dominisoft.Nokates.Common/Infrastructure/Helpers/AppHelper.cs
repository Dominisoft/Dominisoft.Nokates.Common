using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Web.Administration;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class AppHelper
    {
        private static string _appName;
        public static string GetAppName()
        {
            if (!string.IsNullOrEmpty(_appName))
                return _appName;
            var apps = GetApps();
            if (AppDomain.CurrentDomain.BaseDirectory == null) return _appName;
            var exeDir = Environment.ExpandEnvironmentVariables(AppDomain.CurrentDomain.BaseDirectory);
            var dirs = apps.SelectMany(a => a.VirtualDirectories.Select(d => Environment.ExpandEnvironmentVariables(d.PhysicalPath))).ToList();
            var app = apps.FirstOrDefault(app => app.VirtualDirectories.Any(dir => dir.PhysicalPath + "\\" == exeDir));

            _appName= app?.ApplicationPoolName ?? "Unknown Service";

            return _appName;
        }
        public static List<EndpointDescription> GetEndpoints(List<EndpointDataSource> endpointSources)
        {
            var appName = GetAppName();
            var endpoints = endpointSources
              .SelectMany(es => es.Endpoints)
              .OfType<RouteEndpoint>();
            var output = endpoints
                .Where(e =>
                e.Metadata
                        .OfType<ControllerActionDescriptor>()
                        .FirstOrDefault() != null

                ).Select(
                e =>
                {
                    var controller = e.Metadata
                        .OfType<ControllerActionDescriptor>()
                        .FirstOrDefault();
                    var action = controller != null
                        ? $"{controller.ControllerName}.{controller.ActionName}"
                        : null;
                    var controllerMethod = controller != null
                        ? $"{controller?.ControllerTypeInfo?.FullName}.{controller?.MethodInfo?.Name}"
                        : null;
                    var hasNoAuth = controller?.EndpointMetadata?.Any(m => m?.GetType() == typeof(NoAuth))??false;
                    return new EndpointDescription
                    {
                        Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods?[0],
                        Route = $"/{e.RoutePattern.RawText.TrimStart('/')}",
                        Action = action,
                        ControllerMethod = controllerMethod,
                        HasNoAuthAttribute = hasNoAuth, 
                        AppName = appName,
                        Controller = controller?.ControllerName
                    };
                }
            ).ToList();

            return output;

        }

        internal static VersionDetails GetVersionDetails()
        {

            var strExeFilePath = Assembly.GetExecutingAssembly().Location;

            var strWorkPath = Path.GetDirectoryName(strExeFilePath);
            if (File.Exists($"{strWorkPath}\\Version.json"))
            {
                var json = File.ReadAllText($"{strWorkPath}\\Version.json");
                return json.Deserialize<VersionDetails>();
            }
            var @this = Assembly.GetExecutingAssembly();
            return new VersionDetails
            {
                Version = @this.GetName().Version.ToString(),
                BuildDate = @this.GetLinkerTime(),
            };
        }
        private static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int cPeHeaderOffset = 60;
            const int cLinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var read = stream.Read(buffer, 0, 2048);
            }

            var offset = BitConverter.ToInt32(buffer, cPeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + cLinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        public static EndpointDescription GetEndpoint(List<EndpointDataSource> endpointSources,string path)
        {
            var endpoints = GetEndpoints(endpointSources);
            var endpoint = endpoints.FirstOrDefault(e => PathMatches(e,path));

            if (endpoint != null)
            return endpoint;

            ConfigurationValues.TryGetValue(out bool debugMode, "DebugMode");

            if (!debugMode) return null;
            {
                var log = $"Unable to find endpoint for {path}\r\nAvailable Routes:";
                log = endpoints.Aggregate(log, (current, e) => current + $"{e.Route}\r\n");

                StatusValues.Log(log);
            }


            return null;
        }

        private static bool PathMatches(EndpointDescription e, string path)
        {
            var endpointPath = ($"{e.Method}:{e.Route.Before("?")}").ToLower();
            var endpointParts = endpointPath.Split("/");
            var pathParts = path.ToLower().Split("/");
            if (endpointParts.Length != pathParts.Length)
                return false;
            var match = true;

            var length = endpointParts.Length;
            for (var i = 0; i < length; i++)
            {
                var endpointPart = endpointParts[i];
                var pathPart = pathParts[i];
                match = match && (
                    endpointPart == pathPart || endpointPart.StartsWith("{"));
            }

            return match;

        }

        public static ApplicationCollection GetApps()
        {
            var serverManager = new ServerManager();
            var sites = serverManager.Sites;
            var maxCount = sites.Select(s => s.Applications.Count).Max();
            var site = sites.FirstOrDefault(s => s.Applications.Count == maxCount);
            var apps = site?.Applications;
            return apps;

        }
        public static string GetRootUri()
        {
            var serverManager = new ServerManager();
            var sites = serverManager.Sites;
            var maxCount = sites.Select(s => s.Applications.Count).Max();
            var site = sites.FirstOrDefault(s => s.Applications.Count == maxCount);
            if (site == null) return string.Empty;
            var binding = site.Bindings.FirstOrDefault();
            return binding != null ? @$"{binding.Protocol}://{binding.Host}/" : string.Empty;

        }
        public static string GetAppUri()
        {
            return $"{GetRootUri()}{GetAppName()}/";
        }
    }
}
