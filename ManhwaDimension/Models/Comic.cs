using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Comic
    {
        public Comic()
        {
            Bookmarks = new HashSet<Bookmark>();
            Chapters = new HashSet<Chapter>();
            Comments = new HashSet<Comment>();
            Ratings = new HashSet<Rating>();
            Subscriptions = new HashSet<Subscription>();
            Genres = new HashSet<Genre>();
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Slug { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? CoverImageUrl { get; set; }
        public int AuthorId { get; set; }
        public int Status { get; set; }
        public string Language { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public bool Active { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
