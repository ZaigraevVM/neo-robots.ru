using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class NewspaperMap : IEntityTypeConfiguration<Newspaper>
    {
        public void Configure(EntityTypeBuilder<Newspaper> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(e => e.Name).HasMaxLength(500);
        }
    }
}
