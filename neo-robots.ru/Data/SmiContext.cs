using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class SmiContext : IdentityDbContext
    {
        public SmiContext()
        {
        }

        public SmiContext(DbContextOptions<SmiContext> options)
            : base(options)
        {
            //Database.Migrate();
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<HashTag> HashTags { get; set; }
        public virtual DbSet<HashTagsNews> HashTagsNews { get; set; }
        public virtual DbSet<HashTagsTheme> HashTagsThemes { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCities> NewsCities { get; set; }
        public virtual DbSet<NewsRegion> NewsRegions { get; set; }
        public virtual DbSet<NewsTheme> NewsThemes { get; set; }
        public virtual DbSet<Newspaper> Newspapers { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Smi;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(k => k.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.RegionId);
            });

            modelBuilder.Entity<HashTag>(entity =>
            {
                entity.HasKey(k => k.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HashTagsNews>(entity =>
            {
                entity.HasKey(e => new { e.HashTagId, e.NewsId });

                entity.HasOne(d => d.HashTag)
                    .WithMany(p => p.HashTagsNews)
                    .HasForeignKey(d => d.HashTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HashTagsNews_HashTags");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.HashTagsNews)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HashTagsNews_News");
            });

            modelBuilder.Entity<HashTagsTheme>(entity =>
            {
                entity.HasKey(e => new { e.HashTagId, e.ThemeId });

                entity.HasOne(d => d.HashTag)
                    .WithMany(p => p.HashTagsThemes)
                    .HasForeignKey(d => d.HashTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HashTagsThemes_HashTags");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.HashTagsThemes)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HashTagsThemes_Themes");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Text).HasColumnType("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_News_Authors");

                entity.HasOne(d => d.Newspapers)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.NewspapersId)
                    .HasConstraintName("FK_News_Newspapers");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_News_Photos");
            });

            modelBuilder.Entity<NewsCities>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.CityId });

                entity.HasOne(d => d.Cities)
                    .WithMany(p => p.NewsCities)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsCities_Cities");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsCities)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsCities_News");
            });

            modelBuilder.Entity<NewsRegion>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.RegionId });

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsRegions)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsRegions_News");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.NewsRegions)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsRegions_Regions");
            });

            modelBuilder.Entity<NewsTheme>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.ThemeId });

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsThemes)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsThemes_News");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.NewsThemes)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsThemes_Themes");
            });

            modelBuilder.Entity<Newspaper>(entity =>
            {
                entity.HasKey(k => k.Id);

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
