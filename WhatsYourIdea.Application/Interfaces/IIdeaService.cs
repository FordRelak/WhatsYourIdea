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

        Task<IdeaDetailedDto> GetPublicIdeaByHash(string hash, string userName);

        Task DeleteIdeaAsync(string hash, string userName);

        Task<IdeaDetailedDto> GetIdeaForEdit(string hash, string userName);

        Task TrackIdeaAsync(string hash, string name);

        Task<IEnumerable<IdeaDto>> GetUnPublicIdeas();

        Task<IEnumerable<IdeaDto>> SearchIdea(string search, string name);

        Task UnTrackIdeaAsync(string hash, string name);

        Task<IEnumerable<IdeaDto>> GetIdeasByTag(string tagName, string userName);

        Task PublicIdeaAsync(string hash, string name);

        Task VerifiedIdea(string hash);

        Task DeleteIdeaAsync(string hash);

        Task<IdeaDetailedDto> GetIdeaForCheck(string hash);
    }
}