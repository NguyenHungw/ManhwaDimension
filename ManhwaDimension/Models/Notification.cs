using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Type { get; set; } = null!;
        public string PayloadJson { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
