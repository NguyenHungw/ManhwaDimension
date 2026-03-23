using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Report
    {
        public Report()
        {
            ModerationTasks = new HashSet<ModerationTask>();
        }

        public int Id { get; set; }
        public long ReporterId { get; set; }
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = null!;
        public string? ReviewedBy { get; set; }
        public DateTimeOffset? ReviewedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual User Reporter { get; set; } = null!;
        public virtual ICollection<ModerationTask> ModerationTasks { get; set; }
    }
}
