// Дата начала новой эры (14 июля 2024 UTC)
const eraStart = new Date('2024-07-14T00:00:00Z');

const msInDay = 1000 * 60 * 60 * 24;

const getYearAndEra = (diffDays: number): { year: number, era: string } => {
    let year;
    let era;
    if (diffDays >= 25) {
        year = Math.floor(diffDays / 4) + 1;
        era = "";
    } else if (diffDays >= 0) {
        year = Math.ceil(Math.abs(diffDays) / 4);
        era = "новой эры";
    } else {
        year = Math.ceil(Math.abs(diffDays) / 4);
        era = "до н.э";
    }
    return { year, era }
}

const toCent = (year: number, era: string): string => {
    const values = [90, 50, 40, 10, 9, 5, 4, 1];
    const symbols = ['XC', 'L', 'XL', 'X', 'IX', 'V', 'IV', 'I'];
    let result = '';
    let remaining = Math.floor(year / 100) + 1;

    for (let i = 0; i < values.length; i++) {
        while (remaining >= values[i]) {
            result += symbols[i];
            remaining -= values[i];
        }
    }
    return `${result} век ${era}.`;
}

const toYearWithSeason = (year: number, era: string, diffDays: number): string => {
    const seasons = ["Зима", "Весна", "Лето", "Осень"];
    const seasonIndex = (Math.abs(diffDays) % 4);
    const season = seasons[seasonIndex];
    return `${year} год ${era}. ${season}.`;
}

export const ToGameDate = (realDateTime: string): string => {
    const inputDate = new Date(realDateTime);
    const diffMs = inputDate.getTime() - eraStart.getTime();
    const diffDays = Math.floor(diffMs / msInDay);

    const { year, era } = getYearAndEra(diffDays);

    const daysFromNow = Math.floor((Date.now() - inputDate.getTime()) / msInDay);
    if (daysFromNow > 1000) {
        return toCent(year, era);
    }
    else if (daysFromNow > 50) {
        return `${year} год ${era}.`;
    }
    else {
        return toYearWithSeason(year, era, diffDays);
    }
}