using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Users.Attributes
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;

            if (string.IsNullOrEmpty(email))
                return ValidationResult.Success!;

            return !Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")
                ? throw new YagoException("Некорректный формат электронной почты.", 400)
                : ValidationResult.Success!;
        }
    }
}