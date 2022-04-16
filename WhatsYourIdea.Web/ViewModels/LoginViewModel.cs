using System.ComponentModel.DataAnnotations;

namespace WhatsYourIdea.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}