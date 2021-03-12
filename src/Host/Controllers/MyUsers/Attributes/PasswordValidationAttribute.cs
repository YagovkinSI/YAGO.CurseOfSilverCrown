using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace YAGO.World.Host.Controllers.MyUsers.Attributes
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
                return new ValidationResult("Требуется пароль.");

            var errorList = new List<string>();

            if (!Regex.IsMatch(password, "[a-z]"))
                errorList.Add("Пароль должен содержать строчную латинскую букву.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                errorList.Add("Пароль должен содержать заглавную латинскую букву.");

            if (!Regex.IsMatch(password, "[0-9]"))
                errorList.Add("Пароль должен содержать цифру.");

            if (password.Length < 8)
                errorList.Add("Пароль должен содержать не менее 8 символов.");

            if (errorList.Any())
                return new ValidationResult(string.Join(" ", errorList));

            return ValidationResult.Success!;
        }
    }
}