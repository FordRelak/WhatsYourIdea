using WhatsYourIdea.Infrastructure;

namespace WhatsYourIdea.Applications.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWorkInfrastructure _unitOfWork;

        private readonly Lazy<IAuthorService> _authorService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly Lazy<IIdeaService> _ideaService;
        private readonly Lazy<ITagService> _tagService;

        public IAuthorService AuthorService => _authorService.Value;

        public ICommentService CommentService => _commentService.Value;

        public IIdeaService IdeaService => _ideaService.Value;

        public ITagService TagService => _tagService.Value;

        public UnitOfWorkService(IUnitOfWorkInfrastructure unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authorService = new Lazy<IAuthorService>(() => new AuthorService(_unitOfWork.AuthorRepository));
            _commentService = new Lazy<ICommentService>(() => new CommentService(_unitOfWork.CommentRepository));
            _ideaService = new Lazy<IIdeaService>(() => new IdeaService(_unitOfWork.IdeaRepository));
            _tagService = new Lazy<ITagService>(() => new TagService(_unitOfWork.TagRepository));
        }
    }
}