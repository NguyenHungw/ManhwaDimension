using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class ModerationTask
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string? AssignedTo { get; set; }
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ResolvedAt { get; set; }

        public virtual Report Report { get; set; } = null!;
    }
}
