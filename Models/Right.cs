using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Right
    {
        public Right()
        {
            RoleRights = new HashSet<RoleRight>();
        }

        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
