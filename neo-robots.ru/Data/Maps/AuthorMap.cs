using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMI.Data.Entities;

namespace SMI.Data.Maps
{
    public class AuthorMap : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(e => e.FirstName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

            builder.Property(e => e.LastName)
                .HasMaxLength(500)
                .IsUnicode(false);
        }
    }
}
