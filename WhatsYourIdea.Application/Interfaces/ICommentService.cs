namespace WhatsYourIdea.Applications.Services
{
    public interface ICommentService
    {
        Task AddCommentToIdea(string hash, string text, string name);
    }
}