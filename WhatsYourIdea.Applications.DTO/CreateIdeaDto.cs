namespace WhatsYourIdea.Applications.DTO
{
    public class CreateIdeaDto
    {
        public string HashId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public int[] TagIds { get; set; }
        public string NewTags { get; set; }
        public bool IsPrivate { get; set; }
        public string MainImagePath { get; set; }
    }
}