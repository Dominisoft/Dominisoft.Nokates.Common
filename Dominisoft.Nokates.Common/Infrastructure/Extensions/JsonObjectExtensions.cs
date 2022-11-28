using System;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class JsonObjectExtensions
    {
        public static string Serialize<TObj>(this TObj obj)
            => JsonConvert.SerializeObject(obj);
        public static TObj Deserialize<TObj>(this string str)
            => JsonConvert.DeserializeObject<TObj>(str);
        public static TObj TryDeserialize<TObj>(this string str, out string message) where TObj : class
        {
            try
            {
                message = string.Empty;
                return str.Deserialize<TObj>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message = ex.Message;
            }

            return null;
        }

    }
}
