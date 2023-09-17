using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class NewsRegionMap : IEntityTypeConfiguration<NewsRegion>
    {
        public void Configure(EntityTypeBuilder<NewsRegion> builder)
        {
            builder.HasKey(e => new { e.NewsId, e.RegionId });

            builder.HasOne(d => d.News)
                .WithMany(p => p.NewsRegions)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsRegions_News");

            builder.HasOne(d => d.Region)
                .WithMany(p => p.NewsRegions)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsRegions_Regions");
        }
    }
}
