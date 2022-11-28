using System;
using Dominisoft.Nokates.Common.Infrastructure.CustomExceptions;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class RestResponseExtensions
    {
        public static TObject ThrowIfError<TObject>(this RestResponse<TObject> response) where TObject : class
        {
            if (response.IsError)
                throw new BadResponseException(response.Message);
            return response.Object;
        }
    }
}
