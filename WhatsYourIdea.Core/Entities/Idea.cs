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
        public IList<Tag> Tags { get; set; } = new List<Tag>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<UserProfile> TrackingUsers { get; set; } = new List<UserProfile>();
        public string Hash { get; set; }
        public bool IsVerifed { get; set; }
        public string MainImagePath { get; set; }
    }
}