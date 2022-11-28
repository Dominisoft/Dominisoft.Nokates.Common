using Dominisoft.Nokates.Common.Infrastructure.Extensions;

namespace Dominisoft.Nokates.Common.Models
{
    public class RestResponse
    {
        public RestResponse(int httpCode, string message)
        {
            HttpCode = httpCode;
            IsError = HttpCode > 399;
            Message = message;
        }
        public bool IsError { get; set; }
        public int HttpCode { get; set; }
        public string Message { get; set; }
    }

    public class RestResponse<TObject> : RestResponse where TObject : class
    {
        public RestResponse(int httpCode, string message):base(httpCode, message)
        {
            Object = message.TryDeserialize<TObject>(out string error);
        }
        public TObject Object { get; set; }
    }
}
