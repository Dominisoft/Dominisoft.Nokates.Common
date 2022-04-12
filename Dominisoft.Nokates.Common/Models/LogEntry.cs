using System;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;

namespace Dominisoft.Nokates.Common.Models
{
    [DefaultConnectionString("Metrics")]
    public class LogEntry : Entity
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}
