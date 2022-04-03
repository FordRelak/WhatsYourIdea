using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using WhatsYourIdea.Infrastructure.Identity;

namespace Application
{
    public interface IUnitOfWorkInfrastructure
    {
        IRepository<Tag> TagRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Author> AuthorRepository { get; }
        UserManager<ApplicationUser> UserRepository { get; }
        RoleManager<ApplicationRole> RoleRepository { get; }
        IRepository<Idea> IdeaRepository { get; }

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackAsync();

        Task SaveChangesAsync();
    }
}