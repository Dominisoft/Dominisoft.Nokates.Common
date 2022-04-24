using System.ComponentModel.DataAnnotations.Schema;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;

namespace Dominisoft.Nokates.Common.Models
{
    [DefaultConnectionString("Identity")]
    [Table("Users")]

    public class User:Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string AdditionalEndpointPermissions { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get;set; }

    }
}
