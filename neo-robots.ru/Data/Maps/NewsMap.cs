using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class NewsMap : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(e => e.Date).HasColumnType("datetime");

            builder.Property(e => e.Text).HasColumnType("text");
            builder.Property(e => e.Text)
                .HasMaxLength(4000);

            builder.Property(e => e.Title)
                .HasMaxLength(500);

            builder.Property(d => d.Path)
                .HasMaxLength(4000);

            builder.HasIndex(e => e.Path)
                .IsUnique();

            builder.HasOne(d => d.Author)
                .WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_News_Authors");

            builder.HasOne(d => d.Newspapers)
                .WithMany(p => p.News)
                .HasForeignKey(d => d.NewspapersId)
                .HasConstraintName("FK_News_Newspapers");

            builder.HasOne(d => d.Photo)
                .WithMany(p => p.News)
                .HasForeignKey(d => d.PhotoId)
                .HasConstraintName("FK_News_Photos");
        }
    }
}
