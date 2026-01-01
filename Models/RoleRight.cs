using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class RoleRight
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long RightsId { get; set; }
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Right Rights { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
