using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Client;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Configuration
{
    public static class ConfigurationValues
    {
        public static string Token => _token ?? SetToken();
        private static string _token;
        public static string JsonConfig { get; private set; }
        public static Dictionary<string, string> Values { get; private set; }

        public static bool TryGetValue<TValue>(out TValue value, string variableName) where TValue : new()
        {
            var contains = TryGetValue(out var str, variableName);
            value = contains ? str.Deserialize<TValue>() : new TValue();
            return contains;

        }
        public static bool TryGetValue(out string value, string variableName)
        {
            if (! (Values?.ContainsKey(variableName) ?? false))
            {                
                value = string.Empty;
                return false;
            }
            value = Values[variableName];
            return true;

        }
        private static string SetToken()
        {
            TryGetValue(out var authorizationEndpointUrl, "AuthorizationURL");
            authorizationEndpointUrl = "http://DevAppServer/Identity/Authentication";
            var authClient = new AuthenticationClient(authorizationEndpointUrl);
            TryGetValue(out var serviceAccountUsername, "ServiceAccountUsername");
            TryGetValue(out var serviceAccountPassword, "ServiceAccountPassword");
            //TODO: Remove this and find a way to get initial service account to download config
            serviceAccountUsername = "ServiceLogin";
            serviceAccountPassword = "ServicePassword";

            _token = Task.Run(() => authClient.GetToken(serviceAccountUsername,serviceAccountPassword)).Result;
            if (_token != null) return _token;

            StatusValues.Log("Unable to get token");
            return "{}";
        }

     
        internal static async Task LoadConfig(string configServiceName)
        {
            TryGetValue(out bool debugMode, "DebugMode");
            var configUri = $"{AppHelper.GetRootUri()}{configServiceName}/{AppHelper.GetAppName()}";
            if (debugMode)
                StatusValues.Log("Config Path:" + configUri);
            JsonConfig = GetConfig(configUri);
            if (JsonConfig.StartsWith("User does not have permission for endpoint"))
            {
                if (debugMode)
                    StatusValues.Log(JsonConfig);
                return;
            }
            Values = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConfig);
        }
        private static string GetConfig(string url)
        {
            return HttpHelper.Get(url, Token);
            
        }


        internal static void LoadConfigFromFile(string path)
        {

            JsonConfig = File.ReadAllText(path);
            Values = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConfig);
        }
    }
}
