using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Web.Models
{
    public class ProfileModel
    {
        public IEnumerable<IdeaDto> PrivateIdeas { get; set; } = new List<IdeaDto>();
        public IEnumerable<IdeaDto> TrackedIdeas { get; set; } = new List<IdeaDto>();
        public IEnumerable<IdeaDto> PublishedIdeas { get; set; } = new List<IdeaDto>();
    }
}