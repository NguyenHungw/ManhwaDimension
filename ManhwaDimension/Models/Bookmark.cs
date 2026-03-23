using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Bookmark
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int ComicId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual Comic Comic { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
