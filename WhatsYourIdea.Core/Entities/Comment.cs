namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public UserProfile User { get; set; }
        public Idea Idea { get; set; }
    }
}