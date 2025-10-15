using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace YAGO.World.Host.Controllers.Colonies.Attributes
{
    public class ColonyNameValidationAttribute : ValidationAttribute
    {
        private static readonly Regex AllowedChars = new(@"^[A-Za-z0-9\s\-'\.]+$");
        private static readonly Regex NoStartSeparator = new(@"^[A-Za-z0-9]");
        private static readonly Regex NoEndSeparator = new(@"[A-Za-z0-9]$");
        private static readonly Regex NoConsecutiveSeparators = new(@"^[^\.\-\s']*([\.\-\s'][^\.\-\s']+)*[^\.\-\s']*$");

        private static readonly HashSet<string> BannedNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "fuck", "shit", "nigger",
            "system", "admin", "gm", "moderator",
            "capital", "colony", "base", "station", "city"
        };

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var name = value as string;

            if (string.IsNullOrWhiteSpace(name))
                return new ValidationResult("Требуется название колонии.");

            var trimmed = name.Trim();
            var errorList = new List<string>();

            if (trimmed.Length < 3)
                errorList.Add("Название должно содержать минимум 3 символа.");

            if (trimmed.Length > 16)
                errorList.Add("Название должно содержать максимум 16 символов.");

            if (!AllowedChars.IsMatch(trimmed))
                errorList.Add("Разрешены только английские буквы, цифры, пробелы, дефисы, апострофы и точки.");

            if (!NoStartSeparator.IsMatch(trimmed))
                errorList.Add("Название не может начинаться с пробела, дефиса, апострофа или точки.");

            if (!NoEndSeparator.IsMatch(trimmed))
                errorList.Add("Название не может заканчиваться пробелом, дефисом, апострофом или точки.");

            if (!NoConsecutiveSeparators.IsMatch(trimmed))
                errorList.Add("Разделители не могут идти подряд.");

            if (BannedNames.Contains(trimmed))
                errorList.Add("Это название запрещено.");

            var sanitized = Regex.Replace(trimmed, @"\s+", " ");
            if (name != sanitized)
                errorList.Add("Название содержит лишние пробелы.");

            return errorList.Count > 0 ? new ValidationResult(string.Join(" ", errorList)) : ValidationResult.Success!;
        }
    }
}