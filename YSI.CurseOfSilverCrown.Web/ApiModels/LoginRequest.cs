using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Web.ApiModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Требуется логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется пароль")]
        public string Password { get; set; }
    }
}
