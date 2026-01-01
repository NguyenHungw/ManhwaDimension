using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class User
    {
        public User()
        {
            Bookmarks = new HashSet<Bookmark>();
            Comments = new HashSet<Comment>();
            FileAssets = new HashSet<FileAsset>();
            Notifications = new HashSet<Notification>();
            Payments = new HashSet<Payment>();
            Ratings = new HashSet<Rating>();
            ReadingProgresses = new HashSet<ReadingProgress>();
            Reports = new HashSet<Report>();
            Subscriptions = new HashSet<Subscription>();
            Wallets = new HashSet<Wallet>();
        }

        public long Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public string GoogleId { get; set; } = null!;
        public string FacebookId { get; set; } = null!;
        public int Gender { get; set; }
        public long RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual UserProfile UserProfile { get; set; } = null!;
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FileAsset> FileAssets { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
