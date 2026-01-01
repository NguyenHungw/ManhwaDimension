using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Comics = new HashSet<Comic>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }
    }
}
