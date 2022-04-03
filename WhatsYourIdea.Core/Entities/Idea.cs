using System.Collections.Generic;

namespace Domain.Entities
{
    public class Idea : BaseEntity
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDesctiption { get; set; }
        public bool IsPrivate { get; set; }
        public Author Author { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<UserProfile> TrackingUsers { get; set; }
    }
}