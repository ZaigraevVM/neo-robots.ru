using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class HashTagsNewsMap : IEntityTypeConfiguration<HashTagsNews>
    {
        public void Configure(EntityTypeBuilder<HashTagsNews> builder)
        {
            builder.HasKey(e => new { e.HashTagId, e.NewsId });

            builder.HasOne(d => d.HashTag)
                .WithMany(p => p.HashTagsNews)
                .HasForeignKey(d => d.HashTagId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HashTagsNews_HashTags");

            builder.HasOne(d => d.News)
                .WithMany(p => p.HashTagsNews)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HashTagsNews_News");
        }
    }
}
