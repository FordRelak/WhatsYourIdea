using Application;

namespace WhatsYourIdea.Application
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkInfrastructure _unitOfWork;

        private readonly Lazy<IAuthorService> _authorService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly Lazy<IIdeaService> _ideaService;
        private readonly Lazy<ITagService> _tagService;
        private readonly Lazy<IUserService> _userService;

        public IAuthorService AuthorService => _authorService.Value;

        public ICommentService CommentService => _commentService.Value;

        public IIdeaService IdeaService => _ideaService.Value;

        public ITagService TagService => _tagService.Value;

        public IUserService UserService => _userService.Value;

        public UnitOfWorkService(IUnitOfWorkInfrastructure unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authorService = new Lazy<IAuthorService>(() => new AuthorService(_unitOfWork.AuthorRepository));
            _commentService = new Lazy<ICommentService>(() => new CommentService(_unitOfWork.CommentRepository));
            _ideaService = new Lazy<IIdeaService>(() => new IdeaService(_unitOfWork.IdeaRepository));
            _tagService = new Lazy<ITagService>(() => new TagService(_unitOfWork.TagRepository));
            _userService = new Lazy<IUserService>(() => new UserService(_unitOfWork.UserRepository));
        }
    }
}