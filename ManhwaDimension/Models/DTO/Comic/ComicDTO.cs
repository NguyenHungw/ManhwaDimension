using NuGet.Packaging.Signing;

namespace ManhwaDimension.Models.DTO.Comic
{
    // ComicDTO.cs
    public class ComicDTO
    {
        public string Title { get; set; }
        public string? Slug { get; set; } // Nullable, sẽ tự động tạo nếu null
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string CoverImageUrl { get; set; }
        public int AuthorId { get; set; }
        public int Status { get; set; }
        public string Language { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Timestamp RowVersion { get; set; }
        public bool Active { get; set; }
    }
}
