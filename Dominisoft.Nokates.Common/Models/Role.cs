using System.ComponentModel.DataAnnotations.Schema;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;

namespace Dominisoft.Nokates.Common.Models
{
    [DefaultConnectionString("Identity")]
    [Table("Roles")]

    public class Role: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AllowedEndpoints { get; set; }
    }
}
