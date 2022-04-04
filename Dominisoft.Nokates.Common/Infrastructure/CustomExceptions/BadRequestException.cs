namespace Dominisoft.Nokates.Common.Infrastructure.CustomExceptions
{
    public class BadRequestException : RequestException
    {
        public BadRequestException(string message) : base(400, message) { }
    }
}
