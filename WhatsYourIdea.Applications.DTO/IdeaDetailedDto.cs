namespace WhatsYourIdea.Applications.DTO
{
    public class IdeaDetailedDto
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public IEnumerable<TagDto> Tags { get; set; } = Array.Empty<TagDto>();
        public DateTime CreateDate { get; set; }
        public int TrackedNumber { get; set; }
        public int CommentNumber { get; set; }
        public bool IsTracked { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; } = Array.Empty<CommentDto>();
        public UserDto Author { get; set; }
        public string MainImagePath { get; set; }
    }
}