using System.ComponentModel.DataAnnotations;

namespace YAGO.World.Host.Controllers.Common
{
    public class IdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var id = value as long?;

            if (id == null)
                return new ValidationResult("Идентификатор не может быть NULL.");

            if (id < 1)
                return new ValidationResult("Идентификатор не может быть меньше 1.");

            return ValidationResult.Success!;
        }
    }
}