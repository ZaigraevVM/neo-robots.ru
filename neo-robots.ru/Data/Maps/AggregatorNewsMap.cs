using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AggregatorNewsMap : IEntityTypeConfiguration<AggregatorNews>
    {
        public void Configure(EntityTypeBuilder<AggregatorNews> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(e => e.Html).HasColumnType("text");

            builder.HasOne(d => d.News)
                .WithOne(n => n.AggregatorNews)
                .HasForeignKey<News>(e => e.AggregatorNewsId)
                .HasConstraintName("FK_AggregatorNews_AggregatorNews");

            builder.HasOne(d => d.AggregatorSource)
                .WithMany(n=>n.AggregatorNewsList)
                .HasForeignKey(d => d.AggregatorSourceId)
                .HasConstraintName("FK_AggregatorNews_AggregatorSourceId");

            /*
            builder.HasOne(d => d.AggregatorPage)
                .WithMany()
                .HasForeignKey(d => d.AggregatorPageId)
                .HasConstraintName("FK_AggregatorNews_AggregatorPageId")
                .OnDelete(DeleteBehavior.ClientCascade);
            */
        }
    }
}
