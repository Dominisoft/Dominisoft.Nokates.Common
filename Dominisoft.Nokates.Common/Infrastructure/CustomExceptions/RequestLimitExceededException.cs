namespace Dominisoft.Nokates.Common.Infrastructure.CustomExceptions
{
    public class RequestLimitExceededException : RequestException
    {
        public RequestLimitExceededException(string message) : base(429, message) { }
    }
}
