using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AggregatorSourceMap : IEntityTypeConfiguration<AggregatorSource>
    {
        public void Configure(EntityTypeBuilder<AggregatorSource> builder)
        {
            builder.HasKey(k => k.Id);
        }
    }
}
