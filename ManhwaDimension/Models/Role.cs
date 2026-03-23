using ManhwaDimension.Util.Entities;
using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Role : IEntityBase
    {
        public Role()
        {
            RoleMenus = new HashSet<RoleMenu>();
            RoleRights = new HashSet<RoleRight>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
        public string? Color { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        public virtual ICollection<RoleRight> RoleRights { get; set; }
        public virtual ICollection<User> Users { get; set; }
        int IEntityBase.Id { get => throw new NotImplementedException(); set => Id = value; }
        DateTimeOffset IEntityBase.CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
