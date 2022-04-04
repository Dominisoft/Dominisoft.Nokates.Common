using Dominisoft.Nokates.Common.Infrastructure.Configuration;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class LoggingHelper
    {
        public static void LogMessage(string msg)
            => StatusValues.Log(msg);
    }
}
