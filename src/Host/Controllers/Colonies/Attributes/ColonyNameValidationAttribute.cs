using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Colonies.Attributes
{
    public class ColonyNameValidationAttribute : ValidationAttribute
    {
        private static readonly Regex AllowedChars = new(@"^[A-Za-zА-Яа-я0-9\s\-']+$");
        private static readonly Regex NoStartSeparator = new(@"^[A-Za-zА-Яа-я0-9]");
        private static readonly Regex NoEndSeparator = new(@"[A-Za-zА-Яа-я0-9]$");
        private static readonly Regex NoConsecutiveSeparators = new(@"^[^\.\-\s']*([\.\-\s'][^\.\-\s']+)*[^\.\-\s']*$");

        private static readonly HashSet<string> BannedNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "fuck", "shit", "nigger", "system", "admin", "moderator",
            "еба", "ёба","хуй", "пизд", "бля", "система", "админ", "модератор",
            "undefined", "null", "nan"
        };

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var name = value as string;

            if (string.IsNullOrWhiteSpace(name))
                throw new YagoException("Требуется название колонии.", 400);

            var trimmed = name.Trim();
            var errorList = new List<string>();

            if (trimmed.Length < 2)
                errorList.Add("Название должно содержать минимум 2 символа.");

            if (trimmed.Length > 20)
                errorList.Add("Название должно содержать максимум 20 символов.");

            if (!AllowedChars.IsMatch(trimmed))
                errorList.Add("Разрешены только латиницу, кирилицу, цифры, пробелы, дефисы и апострофы.");

            if (!NoStartSeparator.IsMatch(trimmed))
                errorList.Add("Название не может начинаться с пробела, дефиса или апострофа.");

            if (!NoEndSeparator.IsMatch(trimmed))
                errorList.Add("Название не может заканчиваться пробелом, дефисом или апострофом.");

            if (!NoConsecutiveSeparators.IsMatch(trimmed))
                errorList.Add("Разделители не могут идти подряд.");

            if (BannedNames.Contains(trimmed))
                errorList.Add("Это название запрещено.");

            var sanitized = Regex.Replace(trimmed, @"\s+", " ");
            if (name != sanitized)
                errorList.Add("Название содержит лишние пробелы.");

            return errorList.Count > 0
                ? throw new YagoException(string.Join(" ", errorList), 400)
                : ValidationResult.Success!;
        }
    }
}