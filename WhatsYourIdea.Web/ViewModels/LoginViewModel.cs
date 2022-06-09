using System.ComponentModel.DataAnnotations;

namespace WhatsYourIdea.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Обязательное поле")]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool IsRemember { get; set; }
    }
}