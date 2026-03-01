using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Host.Controllers.Common
{
    public class IdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var id = value as long?;

            if (id == null)
                throw new YagoException("Идентификатор не может быть NULL.", 400);

            return id < 1 ? throw new YagoException("Идентификатор не может быть меньше 1.", 400) : ValidationResult.Success!;
        }
    }
}