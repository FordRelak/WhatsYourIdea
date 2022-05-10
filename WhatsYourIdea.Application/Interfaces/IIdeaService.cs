using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Applications.Services
{
    public interface IIdeaService
    {
        Task SaveIdea(CreateIdeaDto ideaDto, CancellationToken cancellationToken);
        Task<IEnumerable<IdeaDto>> GetIdeasAsync(string option, string username = null);
        Task<IEnumerable<IdeaDto>> GetIdeasByUser(string username);
        Task<IEnumerable<IdeaDto>> GetPrivateIdeas(string userName);
        Task<IEnumerable<IdeaDto>> GetTrackedIdeas(string userName);
        Task<IEnumerable<IdeaDto>> GetPublishedIdeas(string userName);
        Task<IdeaDetailedDto> GetPublicIdeaByHash(string hash);
    }
}