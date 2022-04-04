using Microsoft.AspNetCore.Mvc;

namespace Dominisoft.Nokates.Common.Infrastructure.Controllers
{
    public class NokatesControllerBase: ControllerBase
    {
        [HttpGet("Nokates/ControllerHealth")]
        public ActionResult GetControllerHealth()
        {
            return Ok();
        }
    }
}
