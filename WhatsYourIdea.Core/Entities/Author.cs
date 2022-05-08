using System.Collections.Generic;

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public IList<Idea> Ideas { get; set; } = new List<Idea>();
        public UserProfile UserProfile { get; set; }
    }
}