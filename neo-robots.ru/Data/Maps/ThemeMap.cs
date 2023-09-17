using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class ThemeMap : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(500);
        }
    }
}
