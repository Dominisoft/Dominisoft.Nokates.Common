using Dominisoft.Nokates.Common.Infrastructure.Controllers;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Dominisoft.Nokates.Common.Infrastructure.Conventions
{
    public class ApiExplorerVisibilityEnabledConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            StatusController.EndpointGroups = application
                            .GetActions()
                           // .FilterOutWebCommon()
                            .GetEndpointGroups();  
                   
        }
    }
}
