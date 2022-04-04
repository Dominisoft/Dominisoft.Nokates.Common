using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Dominisoft.Nokates.Common.Infrastructure.Controllers
{
    [Route("Nokates/")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public static Dictionary<string, List<string>> EndpointGroups;

        private readonly IEnumerable<EndpointDataSource> _endpointSources;

        public StatusController(
            IEnumerable<EndpointDataSource> endpointSources
        ) => _endpointSources = endpointSources;

        [HttpGet("ServiceStatus")]
        [EndpointGroup("NokatesAdmin")]
        public ActionResult<ServiceStatus> GetStatus() 
            => StatusValues.Status;

        [HttpGet("Log")]
        [EndpointGroup("NokatesAdmin")]
        public ActionResult<List<LogEntry>> GetLog() 
            => StatusValues.EventLog.ToList();

        [HttpGet("Requests")]
        [EndpointGroup("NokatesAdmin")]
        public ActionResult<List<RequestMetric>> GetRequestResponses() 
            => StatusValues.RequestMetrics.ToList();

        [HttpGet("EndpointGroups")]
        [EndpointGroup("NokatesAdmin")]
        public ActionResult<Dictionary<string, List<string>>> GetEndpointGroups() 
            => EndpointGroups;

        [HttpGet("Endpoints")]
        [EndpointGroup("NokatesAdmin")]
        public ActionResult<List<EndpointDescription>> ListAllEndpoints() 
            => AppHelper.GetEndpoints(_endpointSources.ToList());

    }
}
