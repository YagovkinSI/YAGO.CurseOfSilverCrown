using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Users.Attributes
{
    public class LoginValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var login = value as string;
            return string.IsNullOrEmpty(login)
                ? throw new YagoException("Требуется логин.", 400)
                : ValidationResult.Success!;
        }
    }
}