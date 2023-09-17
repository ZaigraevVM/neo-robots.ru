using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.RegionId).ValueGeneratedOnAdd();

            builder.HasOne(d => d.Region)
                .WithMany(p => p.Cities)
                .HasForeignKey(d => d.RegionId);
        }
    }
}
