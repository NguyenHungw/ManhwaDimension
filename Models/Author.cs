using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Author
    {
        public Author()
        {
            Comics = new HashSet<Comic>();
            Subscriptions = new HashSet<Subscription>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
