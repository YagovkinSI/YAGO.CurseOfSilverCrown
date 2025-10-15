export const ValidateColonyName = (name: string): { isValid: boolean; error?: string } => {
    const ALLOWED_CHARS = /^[A-Za-z0-9\s\-'.]+$/;
    const NO_START_SEPARATOR = /^[A-Za-z0-9]/;
    const NO_END_SEPARATOR = /[A-Za-z0-9]$/;
    const NO_CONSECUTIVE_SEPARATORS = /^[^.\-\s']*([.\-\s'][^.\-\s']+)*[^.\-\s']*$/;

    const BANNED_NAMES = [
        "fuck", "shit", "nigger",
        "system", "admin", "gm", "moderator",
        "capital", "colony", "base", "station", "city"
    ];

    if (!name || name.trim().length === 0) {
        return { isValid: false, error: "Название не может быть пустым" };
    }

    const trimmed = name.trim();

    if (trimmed.length < 3) {
        return { isValid: false, error: "Название должно содержать минимум 3 символа" };
    }

    if (trimmed.length > 16) {
        return { isValid: false, error: "Название должно содержать максимум 16 символов" }; // Исправлена опечатка
    }

    if (!ALLOWED_CHARS.test(trimmed)) {
        return { isValid: false, error: "Разрешены только английские буквы, цифры, пробелы, дефисы, апострофы и точки" };
    }

    if (!NO_START_SEPARATOR.test(trimmed)) {
        return { isValid: false, error: "Название не может начинаться с пробела, дефиса, апострофа или точки" };
    }

    if (!NO_END_SEPARATOR.test(trimmed)) {
        return { isValid: false, error: "Название не может заканчиваться пробелом, дефисом, апострофом или точкой" };
    }

    if (!NO_CONSECUTIVE_SEPARATORS.test(trimmed)) {
        return { isValid: false, error: "Разделители не могут идти подряд" };
    }

    const lowerName = trimmed.toLowerCase();
    if (BANNED_NAMES.some(banned => lowerName.includes(banned))) {
        return { isValid: false, error: "Это название запрещено" };
    }

    return {
        isValid: true,
    };
};

export const SanitizeColonyName = (name: string): string => {
    const trimmed = name.trim();
    const sanitized = trimmed.replace(/\s+/g, ' ');
    return sanitized;
};