using Dominisoft.Nokates.Common.Infrastructure.Configuration;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class LoggingHelper
    {
        public static void LogMessage(string msg)
            => StatusValues.Log(msg);

        public static void LogDebug(string msg)
        {
            ConfigurationValues.Values.TryGetValue("IsDebug", out var strIsDebug);
            bool.TryParse(strIsDebug, out var isDebug);
            if (isDebug)
                LogMessage(msg);
        }
    }
}
