using System.Linq;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        
        public static string Before(this string str, string s)
        {
            var parts = str.Split(s.ToCharArray().FirstOrDefault());
            return parts[0];
        }
        public static string Remove(this string str, params string[] strings)
        {
            var result = str;
            foreach (var s in strings)
            {
               result = result.Replace(s,string.Empty);
            }
            return result;
        }
        public static bool ContainsOneOf(this string str, params string[] strings)
            => strings.Any(s => str.Contains(s));
    }
}
