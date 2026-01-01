using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Page
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int PageNumber { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int? FileAssetId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public long? SizeBytes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual Chapter Chapter { get; set; } = null!;
    }
}
