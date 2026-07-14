
const getMinutes = (diffMin: number) => {
    const minutes = diffMin;
    const lastDigit = minutes % 10;
    const lastTwoDigits = minutes % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${minutes} минут назад`;
    if (lastDigit === 1) return `${minutes} минуту назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${minutes} минуты назад`;
    return `${minutes} минут назад`;
}

const getHours = (diffHour: number) => {
    const hours = diffHour;
    const lastDigit = hours % 10;
    const lastTwoDigits = hours % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${hours} часов назад`;
    if (lastDigit === 1) return `${hours} час назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${hours} часа назад`;
    return `${hours} часов назад`;
}

const getDays = (diffDay: number) => {
    const days = diffDay;
    const lastDigit = days % 10;
    const lastTwoDigits = days % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${days} дней назад`;
    if (lastDigit === 1) return `${days} день назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${days} дня назад`;
    return `${days} дней назад`;
}

const getWeeks = (diffDay: number) => {
    const weeks = Math.floor(diffDay / 7);
    const lastDigit = weeks % 10;
    const lastTwoDigits = weeks % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${weeks} недель назад`;
    if (lastDigit === 1) return `${weeks} неделю назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${weeks} недели назад`;
    return `${weeks} недель назад`;
}

const getMonths = (diffDay: number) => {
    const months = Math.floor(diffDay / 30);
    const lastDigit = months % 10;
    const lastTwoDigits = months % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${months} месяцев назад`;
    if (lastDigit === 1) return `${months} месяц назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${months} месяца назад`;
    return `${months} месяцев назад`;
}

const getYears = (diffDay: number) => {
    const years = Math.floor(diffDay / 365);
    const lastDigit = years % 10;
    const lastTwoDigits = years % 100;

    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return `${years} лет назад`;
    if (lastDigit === 1) return `${years} год назад`;
    if (lastDigit >= 2 && lastDigit <= 4) return `${years} года назад`;
    return `${years} лет назад`;
}

export const formatTimeAgo = (date: string): string => {
    if (date == undefined)
        return 'только что' //чтобы не падало

    const now = new Date();
    const past = new Date(date);
    const diffMs = now.getTime() - past.getTime();
    const diffSec = Math.floor(diffMs / 1000);
    const diffMin = Math.floor(diffSec / 60);
    const diffHour = Math.floor(diffMin / 60);
    const diffDay = Math.floor(diffHour / 24);

    if (diffSec < 10)
        return 'только что';

    if (diffSec < 60)
        return `${diffSec} сек. назад`;

    if (diffMin < 60)
        return getMinutes(diffMin);

    if (diffHour < 24)
        return getHours(diffHour);

    if (diffDay < 7)
        return getDays(diffDay);

    if (diffDay < 30)
        return getWeeks(diffDay);

    if (diffDay < 365) 
        return getMonths(diffDay);

    return getYears(diffDay);
};