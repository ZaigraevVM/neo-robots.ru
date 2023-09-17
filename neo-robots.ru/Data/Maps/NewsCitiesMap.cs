using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class NewsCitiesMap : IEntityTypeConfiguration<NewsCities>
    {
        public void Configure(EntityTypeBuilder<NewsCities> builder)
        {
            builder.HasKey(e => new { e.NewsId, e.CityId });

            builder.HasOne(d => d.Cities)
                .WithMany(p => p.NewsCities)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsCities_Cities");

            builder.HasOne(d => d.News)
                .WithMany(p => p.NewsCities)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsCities_News");
        }
    }
}
