using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace YAGO.World.Host.Controllers.MyUsers.Attributes
{
    public class ColonyNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var login = value as string;

            if (string.IsNullOrEmpty(login))
                return new ValidationResult("Требуется название колонии.");

            var errorList = new List<string>();

            if (!Regex.IsMatch(login, "^[а-яА-Яa-zA-Z0-9 -]+$"))
                errorList.Add("Название колонии может содержать только цифры, латинские и русские буквы, пробелы и '-'.");

            if (login.Length < 4)
                errorList.Add("Название колонии должен содержать не менее 3 символов.");
            else if (login.Length > 12)
            {
                errorList.Add("Название колонии должен содержать не более 16 символов.");
            }

            if (errorList.Any())
                return new ValidationResult(string.Join(" ", errorList));

            return ValidationResult.Success!;
        }
    }
}