namespace Dominisoft.Nokates.Common.Infrastructure.CustomExceptions
{
    public class AuthorizationException: RequestException
    {
        public AuthorizationException(string message) : base(401,message) { }
    }
}
