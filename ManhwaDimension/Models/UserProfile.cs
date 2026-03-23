using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class UserProfile
    {
        public long UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Locale { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
