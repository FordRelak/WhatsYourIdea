using System.ComponentModel.DataAnnotations;

namespace WhatsYourIdea.Web.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}