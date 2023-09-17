using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SMI.Data.Entities.Properties;
using SMI.Data.Maps;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class SmiContext : IdentityDbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmiContext()
        {
        }

        public SmiContext(DbContextOptions<SmiContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            Database.Migrate();
            _httpContextAccessor = httpContextAccessor;
            SavingChanges += Auditable_SavingChanges;
            SavingChanges += History_SavingChanges;
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<HashTag> HashTags { get; set; }
        public virtual DbSet<HashTagsNews> HashTagsNews { get; set; }
        public virtual DbSet<HashTagsTheme> HashTagsThemes { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCities> NewsCities { get; set; }
        public virtual DbSet<NewsRegion> NewsRegions { get; set; }
        public virtual DbSet<NewsTheme> NewsThemes { get; set; }
        public virtual DbSet<Newspaper> Newspapers { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<AggregatorSource> AggregatorSources { get; set; }
        public virtual DbSet<AggregatorNews> AggregatorNews { get; set; }
        public virtual DbSet<AggregatorList> AggregatorLists { get; set; }
        public virtual DbSet<AggregatorPage> AggregatorPages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Smi;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.ApplyConfiguration(new AggregatorNewsMap());
            modelBuilder.ApplyConfiguration(new AggregatorListMap());
            modelBuilder.ApplyConfiguration(new AggregatorPageMap());
            modelBuilder.ApplyConfiguration(new AggregatorSourceMap());
            modelBuilder.ApplyConfiguration(new AuthorMap());
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new HashTagMap());
            modelBuilder.ApplyConfiguration(new HashTagsNewsMap());
            modelBuilder.ApplyConfiguration(new HashTagsThemeMap());
            modelBuilder.ApplyConfiguration(new NewsMap());
            modelBuilder.ApplyConfiguration(new NewsCitiesMap());
            modelBuilder.ApplyConfiguration(new NewsRegionMap());
            modelBuilder.ApplyConfiguration(new NewsThemeMap());
            modelBuilder.ApplyConfiguration(new NewspaperMap());
            modelBuilder.ApplyConfiguration(new PhotoMap());
            modelBuilder.ApplyConfiguration(new RegionMap());
            modelBuilder.ApplyConfiguration(new ThemeMap());

            OnModelCreatingPartial(modelBuilder);
        }

        private void Auditable_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            var entries = ChangeTracker.Entries().Where(x =>
                x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            foreach (var entry in entries)
            {
                var now = DateTime.Now;
                var entity = (IAuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = userName;
                }

                entity.ModifiedAt = now;
                entity.ModifiedBy = userName;
            }
        }

        private void History_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            var entries = ChangeTracker.Entries().Where(x =>
                x.Entity is IHistory && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = ((IHistory)entry.Entity);
                var modifiedProperties = entry.Properties
                    .Where(p => p.IsModified && p.Metadata.Name != "History" &&
                        (
                            (p.CurrentValue != null && p.OriginalValue != null && !p.CurrentValue.Equals(p.OriginalValue))
                            || (p.CurrentValue != null && p.OriginalValue == null)
                            || (p.CurrentValue == null && p.OriginalValue != null)
                        )
                    );
                if (modifiedProperties.Any(p => p.Metadata.Name != "ModifiedAt"))
                {
                    string history = $"{DateTime.Now}{Environment.NewLine}";
                    foreach (var property in modifiedProperties.OrderBy(p => p.Metadata.Name))
                    {
                        history += $"{property.Metadata.Name}: {property.OriginalValue} --->>> {property.CurrentValue}{Environment.NewLine}";
                    }
                    entity.History = history + Environment.NewLine + entity.History;
                }
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
