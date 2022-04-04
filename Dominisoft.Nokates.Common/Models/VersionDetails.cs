using System;

namespace Dominisoft.Nokates.Common.Models
{
    public class VersionDetails
    {
        public string Version { get; set; }
        public DateTime BuildDate { get; set; }
        public DateTime DeploymentDate { get; set; }
        public string Branch { get; set; }
        public string Environment { get; set; }
        public string LastCommitId { get; set; }
    }
}
