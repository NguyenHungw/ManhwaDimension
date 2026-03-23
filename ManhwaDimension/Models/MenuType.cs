using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class MenuType
    {
        public MenuType()
        {
            Menus = new HashSet<Menu>();
        }

        public long Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
