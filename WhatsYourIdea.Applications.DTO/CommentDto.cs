namespace WhatsYourIdea.Applications.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public UserDto User { get; set; }
    }
}