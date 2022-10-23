using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;

namespace Dominisoft.Nokates.Common.Models
{
    [DefaultConnectionString("Metrics")]
    [Table("RepositoryMetrics")]
    public class RepositoryMetric:Entity
    {
        public Guid RequestTrackingId { get; set; }
        public string ServiceName { get; set; }
        public string Query { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime RequestStart { get; set; }
        /// <summary>
        /// The time in ms for the request to be processed
        /// </summary>
        public long ResponseTime { get; set; }
    }
}
