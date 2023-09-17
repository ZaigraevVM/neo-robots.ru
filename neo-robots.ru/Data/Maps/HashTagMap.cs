using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class HashTagMap : IEntityTypeConfiguration<HashTag>
    {
        public void Configure(EntityTypeBuilder<HashTag> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
        }
    }
}
