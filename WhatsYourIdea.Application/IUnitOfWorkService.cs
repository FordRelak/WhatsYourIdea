namespace WhatsYourIdea.Applications.Services
{
    public interface IUnitOfWorkService
    {
        ITagService TagService { get; }
        IAuthorService AuthorService { get; }
        ICommentService CommentService { get; }
        IIdeaService IdeaService { get; }
    }
}