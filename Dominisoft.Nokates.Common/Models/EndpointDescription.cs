namespace Dominisoft.Nokates.Common.Models
{
    public  class EndpointDescription
    {
        public string Method { get; set; }
        public string Route { get; set; }
        public string Action { get; set; }
        public string ControllerMethod { get; set; }
        public string Controller { get; set; }
        public bool HasNoAuthAttribute { get; set; }
        public string AppName { get; set; }
    }
}
