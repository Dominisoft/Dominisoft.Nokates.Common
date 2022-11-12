using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static ActionResult<bool> ToActionResult(this Entity e)
        {
            if (e?.Id > 0) return true;
            if (e != null) return false;
            return new NotFoundResult();
        }
    }
}
