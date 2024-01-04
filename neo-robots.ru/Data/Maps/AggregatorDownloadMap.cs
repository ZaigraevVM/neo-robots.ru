using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AggregatorDownloadMap : IEntityTypeConfiguration<AggregatorDownload>
    {
        public void Configure(EntityTypeBuilder<AggregatorDownload> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(d => d.AggregatorList)
                .WithMany()
                .HasForeignKey(d => d.AggregatorListId);

            builder.HasOne(d => d.AggregatorPage)
                .WithMany()
                .HasForeignKey(d => d.AggregatorPageId);
        }
    }
}