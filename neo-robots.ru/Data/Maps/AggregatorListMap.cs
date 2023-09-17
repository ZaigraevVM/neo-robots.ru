using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AggregatorListMap : IEntityTypeConfiguration<AggregatorList>
    {
        public void Configure(EntityTypeBuilder<AggregatorList> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(d => d.AggregatorSource)
                .WithMany(a => a.AggregatorLists)
                .HasForeignKey(d => d.AggregatorSourceId);
        }
    }
}
