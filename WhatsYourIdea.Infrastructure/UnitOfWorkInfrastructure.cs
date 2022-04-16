using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Infrastructure
{
    public sealed class UnitOfWorkInfrastructure : IUnitOfWorkInfrastructure
    {
        #region Fields

        private readonly EfDbContext _context;
        private readonly Lazy<IRepository<Tag>> _tagLazy;
        private readonly Lazy<IRepository<Comment>> _commentLazy;
        private readonly Lazy<IRepository<Author>> _authorLazy;
        private readonly Lazy<IRepository<Idea>> _ideaLazy;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private IDbContextTransaction _transaction;

        #endregion Fields

        #region Properties

        public IRepository<Tag> TagRepository => _tagLazy.Value;
        public IRepository<Comment> CommentRepository => _commentLazy.Value;
        public IRepository<Author> AuthorRepository => _authorLazy.Value;
        public IRepository<Idea> IdeaRepository => _ideaLazy.Value;
        public UserManager<ApplicationUser> UserRepository => _userManager;
        public RoleManager<ApplicationRole> RoleRepository => _roleManager;

        #endregion Properties

        public UnitOfWorkInfrastructure(EfDbContext context,
                                        UserManager<ApplicationUser> userManager,
                                        RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _tagLazy = new Lazy<IRepository<Tag>>(() => new Repository<Tag>(context));
            _commentLazy = new Lazy<IRepository<Comment>>(() => new Repository<Comment>(context));
            _authorLazy = new Lazy<IRepository<Author>>(() => new Repository<Author>(context));
            _ideaLazy = new Lazy<IRepository<Idea>>(() => new Repository<Idea>(context));
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Public Methods

        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if(_transaction is not null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if(_transaction is not null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        #endregion Public Methods
    }
}