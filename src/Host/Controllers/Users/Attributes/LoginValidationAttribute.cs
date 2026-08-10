using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Users.Attributes
{
    public class LoginValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var login = value as string;

            if (string.IsNullOrEmpty(login))
                throw new YagoException("Требуется логин.", 400);

            if (login.Length < 3)
                throw new YagoException("Логин должен содержать не менее 3 символов.", 400);
            else if (login.Length > 20)
                throw new YagoException("Логин должен содержать не более 20 символов.", 400);

            var errorList = new List<string>();

            if (!Regex.IsMatch(login, "^[a-zA-Z0-9_-]+$"))
                errorList.Add("Логин может содержать только латинские буквы, цифры, подчеркивание (_) и дефис (-).");

            if (!Regex.IsMatch(login, "[a-zA-Z]"))
                errorList.Add("Логин должен содержать хотя бы одну латинскую букву.");

            return errorList.Any()
                ? throw new YagoException(string.Join(" ", errorList), 400)
                : ValidationResult.Success!;
        }
    }
}