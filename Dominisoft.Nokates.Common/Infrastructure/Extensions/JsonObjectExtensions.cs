using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class JsonObjectExtensions
    {
        public static string Serialize<TObj>(this TObj obj)
            => JsonConvert.SerializeObject(obj);
        public static TObj Deserialize<TObj>(this string str)
                    => JsonConvert.DeserializeObject<TObj>(str);

    }
}
