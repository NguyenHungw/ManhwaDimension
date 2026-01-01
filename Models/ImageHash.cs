using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class ImageHash
    {
        public int Id { get; set; }
        public string Hash { get; set; } = null!;
        public int FileAssetId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public long? SizeBytes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual FileAsset FileAsset { get; set; } = null!;
    }
}
