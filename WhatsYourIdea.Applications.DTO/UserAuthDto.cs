namespace WhatsYourIdea.Applications.DTO
{
    public class UserAuthDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsRemember { get; set; }
    }
}