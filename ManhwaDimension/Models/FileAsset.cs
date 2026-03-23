using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class FileAsset
    {
        public FileAsset()
        {
            ImageHashes = new HashSet<ImageHash>();
        }

        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long? SizeBytes { get; set; }
        public string? Checksum { get; set; }
        public long? UploadedBy { get; set; }
        public DateTimeOffset UploadedAt { get; set; }

        public virtual User? UploadedByNavigation { get; set; }
        public virtual ICollection<ImageHash> ImageHashes { get; set; }
    }
}
