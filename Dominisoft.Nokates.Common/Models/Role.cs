namespace Dominisoft.Nokates.Common.Models
{
    public class Role: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AllowedEndpoints { get; set; }
    }
}
