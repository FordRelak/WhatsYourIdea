using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Web.Models
{
    public class AdminModel
    {
        public IEnumerable<IdeaDto> UnVerifiedIdeas { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<IdeaDto> PublishedIdeas { get; internal set; }
    }
}