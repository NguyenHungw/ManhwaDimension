using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Comments = new HashSet<Comment>();
            Pages = new HashSet<Page>();
            ReadingProgresses = new HashSet<ReadingProgress>();
        }

        public int Id { get; set; }
        public int ComicId { get; set; }
        public string? Title { get; set; }
        public int ChapterNumber { get; set; }
        public int? VolumeNumber { get; set; }
        public string? Summary { get; set; }
        public DateTimeOffset? ReleaseDate { get; set; }
        public bool? IsVisible { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Comic Comic { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; }
    }
}
