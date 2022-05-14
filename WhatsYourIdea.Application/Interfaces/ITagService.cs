using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Applications.Services
{
    public interface ITagService
    {
        IEnumerable<TagDto> GetTags();

        Task<IEnumerable<TagDto>> GetTagsExceptIdsAsync(int[] ids);
    }
}