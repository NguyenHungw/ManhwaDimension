using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class RoleMenu
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
