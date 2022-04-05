using System;

namespace Dominisoft.Nokates.Common.Models
{
    public class ServiceStatus
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsOnline { get; set; }
        public DeploymentStatus DeploymentStatus { get; set; }
        public string Uri { get; set; }
        public VersionDetails Version { get; set; }
    }
}
