using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominisoft.Nokates.Common.Models
{
    public class Entity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
