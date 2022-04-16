using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WhatsYourIdea.Infrastructure
{
    public interface IEfDbContext
    {
        DbSet<Idea> Ideas { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }

        Task SaveChangesAsync();
    }
}