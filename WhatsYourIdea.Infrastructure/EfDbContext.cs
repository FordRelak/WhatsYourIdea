using Application.Configurations;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatsYourIdea.Infrastructure.Configurations;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Infrastructure
{
    public sealed class EfDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IEfDbContext
    {
        #region Tables

        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        #endregion Tables

        public EfDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            AddTime();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTime();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql().UseSnakeCaseNamingConvention();
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdeaConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TagConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfiguration).Assembly);
        }
        private void AddTime()
        {
            var entries = ChangeTracker
                               .Entries()
                               .Where(e => e.Entity is BaseEntity && (
                                       e.State == EntityState.Added
                                       || e.State == EntityState.Modified));

            foreach(var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).Updated = DateTime.UtcNow;

                if(entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                }
            }
        }
    }
}