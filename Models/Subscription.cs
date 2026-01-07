using ManhwaDimension.Util.Entities;
using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Subscription 
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int? ComicId { get; set; }
        public int? AuthorId { get; set; }
        public string Type { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public bool Active { get; set; }

        public virtual Author? Author { get; set; }
        public virtual Comic? Comic { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
