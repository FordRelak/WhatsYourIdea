using System.Collections.Generic;

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public IList<Idea> Ideas { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}