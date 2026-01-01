using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseParentComment = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int? ComicId { get; set; }
        public int? ChapterId { get; set; }
        public long UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Content { get; set; } = null!;
        public bool IsHidden { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual Comic? Comic { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> InverseParentComment { get; set; }
    }
}
