using System.ComponentModel.DataAnnotations;

namespace WhatsYourIdea.Web.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public string Hash { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}