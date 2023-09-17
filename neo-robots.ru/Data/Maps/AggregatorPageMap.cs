using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AggregatorPageMap : IEntityTypeConfiguration<AggregatorPage>
    {
        public void Configure(EntityTypeBuilder<AggregatorPage> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(d => d.AggregatorSource)
                .WithMany(a => a.AggregatorPages)
                .HasForeignKey(d => d.AggregatorSourceId);
        }
    }
}
