using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;

            if (string.IsNullOrEmpty(email))
                return ValidationResult.Success!;

            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
                return new ValidationResult("Некорректный формат электронной почты.");

            return ValidationResult.Success!;
        }
    }
}