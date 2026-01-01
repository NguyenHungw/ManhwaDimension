using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class EventLog
    {
        public long Id { get; set; }
        public string EventType { get; set; } = null!;
        public string? PayloadJson { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
