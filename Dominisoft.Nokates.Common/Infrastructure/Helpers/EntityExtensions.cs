using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class EntityExtensions
    {
        public static string GetTableName(this Entity entity)
        {
            var entityType = entity.GetType();
            var attribute = (TableAttribute)entityType.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(TableAttribute));
            return attribute?.Name??entityType.Name+"s";
        }
    }
}
