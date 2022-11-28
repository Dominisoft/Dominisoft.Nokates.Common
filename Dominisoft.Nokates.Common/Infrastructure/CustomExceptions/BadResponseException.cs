using System;

namespace Dominisoft.Nokates.Common.Infrastructure.CustomExceptions
{
    public class BadResponseException : RequestException
    {
        public BadResponseException(int statusCode, string message) : base(statusCode, message)
        {
        }
        public BadResponseException(string message) : this(417, message)
        {
        }

    }
}
