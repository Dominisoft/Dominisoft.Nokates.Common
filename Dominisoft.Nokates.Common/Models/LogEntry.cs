using System;

namespace Dominisoft.Nokates.Common.Models
{
    public class LogEntry : Entity
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}
