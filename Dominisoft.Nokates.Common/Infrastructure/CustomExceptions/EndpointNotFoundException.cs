namespace Dominisoft.Nokates.Common.Infrastructure.CustomExceptions
{
    public class EndpointNotFoundException : RequestException
    {
        public EndpointNotFoundException(string message) : base(404, message) { }
    }
}
