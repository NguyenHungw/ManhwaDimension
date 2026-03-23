using ManhwaDimension.Util.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhwaDimension.Models
{
    public partial class Tag : IEntityBase
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
        public bool Active { get;set; }
    }
}
