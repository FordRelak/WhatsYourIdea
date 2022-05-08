using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public Author Author { get; set; }
        public string AvatarFilePath { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<Idea> TrackedIdeas { get; set; } = new List<Idea>();
    }
}