using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class AnalyticsEvent
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string EventName { get; set; } = null!;
        public string? PropertiesJson { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
