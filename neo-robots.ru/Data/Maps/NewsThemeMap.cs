using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class NewsThemeMap : IEntityTypeConfiguration<NewsTheme>
    {
        public void Configure(EntityTypeBuilder<NewsTheme> builder)
        {
            builder.HasKey(e => new { e.NewsId, e.ThemeId });

            builder.HasOne(d => d.News)
                .WithMany(p => p.NewsThemes)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsThemes_News");

            builder.HasOne(d => d.Theme)
                .WithMany(p => p.NewsThemes)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsThemes_Themes");
        }
    }
}
