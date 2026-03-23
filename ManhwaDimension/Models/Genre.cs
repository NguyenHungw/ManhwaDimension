using ManhwaDimension.ULT.Entities;
using ManhwaDimension.Util.Entities;
using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Genre : IEntityBase
    {
        public Genre()
        {
            Comics = new HashSet<Comic>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }
    }
}
