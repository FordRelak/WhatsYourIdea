using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WhatsYourIdea.Web.ViewModels
{
    public class EditorViewModel
    {
        public string HashId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string SubTitle { get; set; }

        [Required]
        [AllowHtml]
        public string Text { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile MainImage { get; set; }

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Tags { get; set; }
        public int[] TagIds { get; set; }
        public string NewTags { get; set; }
        public bool IsPrivate { get; set; }
    }
}