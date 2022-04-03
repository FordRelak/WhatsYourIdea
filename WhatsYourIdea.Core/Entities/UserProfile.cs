using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public Author Author { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<Idea> TrackedIdeas { get; set; }
    }
}