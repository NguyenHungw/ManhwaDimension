using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class ReadingProgress
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int ChapterId { get; set; }
        public int LastPageNumber { get; set; }
        public DateTimeOffset LastViewedAt { get; set; }

        public virtual Chapter Chapter { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
