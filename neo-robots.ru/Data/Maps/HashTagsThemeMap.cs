using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class HashTagsThemeMap : IEntityTypeConfiguration<HashTagsTheme>
    {
        public void Configure(EntityTypeBuilder<HashTagsTheme> builder)
        {
            builder.HasKey(e => new { e.HashTagId, e.ThemeId });

            builder.HasOne(d => d.HashTag)
                .WithMany(p => p.HashTagsThemes)
                .HasForeignKey(d => d.HashTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HashTagsThemes_HashTags");

            builder.HasOne(d => d.Theme)
                .WithMany(p => p.HashTagsThemes)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HashTagsThemes_Themes");
        }
    }
}
