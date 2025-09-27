using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes
{
    public class LoginValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var login = value as string;

            if (string.IsNullOrEmpty(login))
                return new ValidationResult("Требуется имя пользователя.");

            var errorList = new List<string>();

            if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
                errorList.Add("Имя пользователя может содержать только цифры и латинские буквы.");

            if (login.Length < 4)
                errorList.Add("Имя пользователя должен содержать не менее 4 символов.");
            else if (login.Length > 12)
            {
                errorList.Add("Имя пользователя должен содержать не более 12 символов.");
            }

            if (errorList.Any())
                return new ValidationResult(string.Join(" ", errorList));

            return ValidationResult.Success!;
        }
    }
}