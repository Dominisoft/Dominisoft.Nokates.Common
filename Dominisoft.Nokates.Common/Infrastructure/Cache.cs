using System;
using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure
{
    public static class Cache
    {
        private static readonly List<CacheEntry> Entries = new List<CacheEntry>();


        public static bool HasValidValue<TParams>(string name, TParams param)
            where TParams : class, new()
            => Entries.Where(e => e.Expires < DateTime.Now).Any(e => e.Name == name && param.Serialize() == e.Params);
        public static bool HasValidValue(string name)
            => Entries.Where(e => e.Expires < DateTime.Now).Any(e => e.Name == name && e.Params == "{}");

        public static bool SetValue<TParams, TValue>(string name, TParams param, TValue value, int cacheTime)
        {
            var expires = DateTime.Now.AddMinutes(cacheTime);
            LoggingHelper.LogDebug($"SetValueWithParams - {name} - Expires {expires}");

            Invalidate(name,param);
            Entries.Add(new CacheEntry
            {
                Expires = DateTime.Now.AddMinutes(cacheTime),
                Name = name,
                Params = param?.Serialize() ?? "{}",
                Result = value.Serialize()
            });
            return true;
        }
        public static bool SetValue< TValue>(string name, TValue value, int cacheTime)
        {
            var expires = DateTime.Now.AddMinutes(cacheTime);
            LoggingHelper.LogDebug($"SetValue - {name} - Expires {expires}");

            Invalidate(name);
            Entries.Add(new CacheEntry
            {
                Expires = expires,
                Name = name,
                Params = "{}",
                Result = value.Serialize()
            });
            return true;
        }


        public static TValue GetValue<TParams, TValue>(string name, TParams param) where TValue : class
            => Entries
                .OrderByDescending(e => e.Expires)
                .Where(e => e.Expires < DateTime.Now)
                ?.FirstOrDefault(e => e.Name == name && e.Params == (param?.Serialize() ?? "{}"))
                ?.Result
                .Deserialize<TValue>();
        public static TValue GetValue< TValue>(string name) where TValue : class
            => Entries
                .OrderByDescending(e => e.Expires)
                .Where(e => e.Expires < DateTime.Now)
                ?.FirstOrDefault(e => e.Name == name && e.Params ==  "{}")
                ?.Result
                .Deserialize<TValue>();

        public static void Invalidate(string name)
        {
            Entries.RemoveAll(e => e.Name == name && e.Params == "{}");
            Entries.RemoveAll(e => e.Expires > DateTime.Now);
        }
        public static void Invalidate<TParam>(string name,TParam param)
        {
            Entries.RemoveAll(e => e.Name == name && e.Params == param.Serialize());
            Entries.RemoveAll(e => e.Expires > DateTime.Now);
        }
    }
}
