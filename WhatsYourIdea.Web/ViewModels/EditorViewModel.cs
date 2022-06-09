using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WhatsYourIdea.Web.ViewModels
{
    public class EditorViewModel
    {
        public string HashId { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Заголовок")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [AllowHtml]
        public string Text { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Баннер")]
        public IFormFile MainImage { get; set; }

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Tags { get; set; }
        public int[] TagIds { get; set; }

        [Display(Name = "Новый тег")]
        public string NewTags { get; set; }

        [Display(Name = "Идея приватная?")]
        public bool IsPrivate { get; set; }
    }
}