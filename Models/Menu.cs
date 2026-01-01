using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Menu
    {
        public Menu()
        {
            RoleMenus = new HashSet<RoleMenu>();
        }

        public long Id { get; set; }
        public int? GroupId { get; set; }
        public int? ParentId { get; set; }
        public bool Active { get; set; }
        public int? Priority { get; set; }
        public long? MenuTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual MenuType? MenuType { get; set; }
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
