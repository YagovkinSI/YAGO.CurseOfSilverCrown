export const ValidateColonyName = (name: string): { isValid: boolean; error?: string } => {
    const ALLOWED_CHARS = /^[A-Za-zА-Яа-я0-9\s\-']+$/;
    const NO_START_SEPARATOR = /^[A-Za-zА-Яа-я0-9]/;
    const NO_END_SEPARATOR = /[A-Za-zА-Яа-я0-9]$/;
    const NO_CONSECUTIVE_SEPARATORS = /^[^.\-\s']*([.\-\s'][^.\-\s']+)*[^.\-\s']*$/;

    const BANNED_NAMES = [
        "fuck", "shit", "nigger", "system", "admin", "moderator",
        "еба", "ёба","хуй", "пизд", "бля", "система", "админ", "модератор",
        "undefined", "null", "nan"
    ];

    if (!name || name.trim().length == 0) {
        return { isValid: false, error: "Название не может быть пустым" };
    }

    const trimmed = name.trim();

    if (trimmed.length < 2) {
        return { isValid: false, error: "Название должно содержать минимум 2 символа" };
    }

    if (trimmed.length > 20) {
        return { isValid: false, error: "Название должно содержать максимум 20 символов" };
    }

    if (!ALLOWED_CHARS.test(trimmed)) {
        return { isValid: false, error: "Разрешены только латиница, кирилица, цифры, пробелы, дефисы и апострофы" };
    }

    if (!NO_START_SEPARATOR.test(trimmed)) {
        return { isValid: false, error: "Название не может начинаться с пробела, дефиса и апострофа" };
    }

    if (!NO_END_SEPARATOR.test(trimmed)) {
        return { isValid: false, error: "Название не может заканчиваться пробелом, дефисом и апострофом" };
    }

    if (!NO_CONSECUTIVE_SEPARATORS.test(trimmed)) {
        return { isValid: false, error: "Разделители (пробел, дефис и апостроф) не могут идти подряд" };
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