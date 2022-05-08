namespace WhatsYourIdea.Applications.DTO
{
    public class IdeaDto
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public string CreateDate { get; set; }
        public int TrackedNumber { get; set; }
        public int CommentNumber { get; set; }
        public bool IsTracked { get; set; }
    }
}