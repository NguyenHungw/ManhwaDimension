using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? ChangedBy { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
        public string? ChangesJson { get; set; }
    }
}
