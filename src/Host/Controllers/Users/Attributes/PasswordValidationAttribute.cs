using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Users.Attributes
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
                throw new YagoNotValidException("Требуется пароль.");

            return password.Length > 20
                ? throw new YagoNotValidException("Пароль должен содержать не более 20 символов.")
                : ValidationResult.Success!;
        }
    }
}