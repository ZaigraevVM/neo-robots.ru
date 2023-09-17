using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class PhotoMap : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

            builder.Property(e => e.FileName)
                .HasMaxLength(100);
        }
    }
}
