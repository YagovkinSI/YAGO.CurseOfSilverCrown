using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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

            if (password.Length < 6)
                throw new YagoNotValidException("Пароль должен содержать не менее 6 символов.");
            else if (password.Length > 20)
                throw new YagoNotValidException("Пароль должен содержать не более 20 символов.");

            var errorList = new List<string>();

            if (!Regex.IsMatch(password, "[a-z]"))
                errorList.Add("Пароль должен содержать строчную латинскую букву.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                errorList.Add("Пароль должен содержать заглавную латинскую букву.");

            if (!Regex.IsMatch(password, "[0-9]"))
                errorList.Add("Пароль должен содержать цифру.");

            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()\-_=+[\]{};:,./?~`""']+$"))
                errorList.Add("Пароль содержит недопустимые символы");

            return errorList.Any()
                ? throw new YagoNotValidException(string.Join(" ", errorList))
                : ValidationResult.Success!;
        }
    }
}