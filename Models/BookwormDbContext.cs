using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ManhwaDimension.Models
{
    public partial class BookwormDbContext : DbContext
    {
        public BookwormDbContext()
        {
        }

        public BookwormDbContext(DbContextOptions<BookwormDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdsCampaign> AdsCampaigns { get; set; } = null!;
        public virtual DbSet<AnalyticsEvent> AnalyticsEvents { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<Comic> Comics { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<EventLog> EventLogs { get; set; } = null!;
        public virtual DbSet<FileAsset> FileAssets { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<ImageHash> ImageHashes { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MenuType> MenuTypes { get; set; } = null!;
        public virtual DbSet<ModerationTask> ModerationTasks { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Page> Pages { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<ReadingProgress> ReadingProgresses { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Right> Rights { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleMenu> RoleMenus { get; set; } = null!;
        public virtual DbSet<RoleRight> RoleRights { get; set; } = null!;
        public virtual DbSet<SearchIndex> SearchIndices { get; set; } = null!;
        public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=ManhwaDimension;User ID=Hungw;Password=123456;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<AdsCampaign>(entity =>
            {
                entity.Property(e => e.Advertiser).HasMaxLength(200);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<AnalyticsEvent>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.EventName).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(100);

                entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.ChangedBy).HasMaxLength(450);

                entity.Property(e => e.EntityId).HasMaxLength(200);

                entity.Property(e => e.EntityName).HasMaxLength(200);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Bio).HasMaxLength(2000);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Slug).HasMaxLength(200);

                entity.Property(e => e.Website).HasMaxLength(500);
            });

            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.ComicId }, "IX_Bookmarks_UserComic")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Comic)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.ComicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookmarks_Comics");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookmarks_Users");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasIndex(e => new { e.ComicId, e.ChapterNumber }, "IX_Chapters_Comic_Chapter")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.IsVisible)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Summary).HasMaxLength(2000);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.HasOne(d => d.Comic)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.ComicId)
                    .HasConstraintName("FK_Chapters_Comics");
            });

            modelBuilder.Entity<Comic>(entity =>
            {
                entity.HasIndex(e => e.Slug, "IX_Comics_Slug")
                    .IsUnique();

                entity.HasIndex(e => e.Title, "IX_Comics_Title");

                entity.Property(e => e.CoverImageUrl).HasMaxLength(1000);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Language)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('en')");

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.ShortDescription).HasMaxLength(1000);

                entity.Property(e => e.Slug).HasMaxLength(300);

                entity.Property(e => e.Title).HasMaxLength(300);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Comics)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comics_Authors");

                entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Comics)
                    .UsingEntity<Dictionary<string, object>>(
                        "ComicGenre",
                        l => l.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CG_Genres"),
                        r => r.HasOne<Comic>().WithMany().HasForeignKey("ComicId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CG_Comics"),
                        j =>
                        {
                            j.HasKey("ComicId", "GenreId").HasName("PK__ComicGen__48C8C039BF5ACFD1");

                            j.ToTable("ComicGenres");
                        });

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Comics)
                    .UsingEntity<Dictionary<string, object>>(
                        "ComicTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CT_Tags"),
                        r => r.HasOne<Comic>().WithMany().HasForeignKey("ComicId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CT_Comics"),
                        j =>
                        {
                            j.HasKey("ComicId", "TagId").HasName("PK__ComicTag__6EA75FF431FD2ED7");

                            j.ToTable("ComicTags");
                        });
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Content).HasMaxLength(2000);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Comments_Chapters");

                entity.HasOne(d => d.Comic)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ComicId)
                    .HasConstraintName("FK_Comments_Comics");

                entity.HasOne(d => d.ParentComment)
                    .WithMany(p => p.InverseParentComment)
                    .HasForeignKey(d => d.ParentCommentId)
                    .HasConstraintName("FK_Comments_Parent");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.ToTable("EventLog");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.EventType).HasMaxLength(200);
            });

            modelBuilder.Entity<FileAsset>(entity =>
            {
                entity.Property(e => e.Checksum).HasMaxLength(200);

                entity.Property(e => e.ContentType).HasMaxLength(100);

                entity.Property(e => e.FileName).HasMaxLength(300);

                entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Url).HasMaxLength(1000);

                entity.HasOne(d => d.UploadedByNavigation)
                    .WithMany(p => p.FileAssets)
                    .HasForeignKey(d => d.UploadedBy)
                    .HasConstraintName("FK_FileAssets_Users");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<ImageHash>(entity =>
            {
                entity.HasIndex(e => e.Hash, "IX_ImageHashes_Hash");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Hash).HasMaxLength(200);

                entity.HasOne(d => d.FileAsset)
                    .WithMany(p => p.ImageHashes)
                    .HasForeignKey(d => d.FileAssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IH_FileAssets");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.MenuType)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.MenuTypeId)
                    .HasConstraintName("FK_Menu_MenuType");
            });

            modelBuilder.Entity<MenuType>(entity =>
            {
                entity.ToTable("MenuType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<ModerationTask>(entity =>
            {
                entity.Property(e => e.AssignedTo).HasMaxLength(450);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ModerationTasks)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MT_Reports");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Type).HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notif_Users");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasIndex(e => new { e.ChapterId, e.PageNumber }, "IX_Pages_Order");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Pages_Chapters");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Currency).HasMaxLength(10);

                entity.Property(e => e.Provider).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TransactionId).HasMaxLength(200);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_Users");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.ComicId }, "IX_Ratings_UserComic")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Review).HasMaxLength(2000);

                entity.HasOne(d => d.Comic)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.ComicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ratings_Comics");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ratings_Users");
            });

            modelBuilder.Entity<ReadingProgress>(entity =>
            {
                entity.ToTable("ReadingProgress");

                entity.HasIndex(e => new { e.UserId, e.ChapterId }, "IX_RP_User_Chapter")
                    .IsUnique();

                entity.Property(e => e.LastViewedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RP_Chapters");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RP_Users");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.Property(e => e.ReviewedBy).HasMaxLength(450);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Pending')");

                entity.Property(e => e.TargetType).HasMaxLength(100);

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reports_Users");
            });

            modelBuilder.Entity<Right>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.ToTable("RoleMenu");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_Menu");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_Role");
            });

            modelBuilder.Entity<RoleRight>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Rights)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RightsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Rights");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Role");
            });

            modelBuilder.Entity<SearchIndex>(entity =>
            {
                entity.ToTable("SearchIndex");

                entity.Property(e => e.EntityType).HasMaxLength(50);

                entity.Property(e => e.Language).HasMaxLength(10);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Sub_Authors");

                entity.HasOne(d => d.Comic)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.ComicId)
                    .HasConstraintName("FK_Sub_Comics");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sub_Users");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Slug).HasMaxLength(150);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Reference).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Wallets");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Role");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserProf__1788CC4C61D41912");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.AvatarUrl).HasMaxLength(1000);

                entity.Property(e => e.Bio).HasMaxLength(1000);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Locale).HasMaxLength(10);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserProfile)
                    .HasForeignKey<UserProfile>(d => d.UserId)
                    .HasConstraintName("FK_UserProfiles_Users");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallets_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
