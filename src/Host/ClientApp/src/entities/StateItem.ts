import { AttachMoney, Balance, Info, People, RocketLaunch, SentimentSatisfied, ViewModule, WorkspacePremium } from "@mui/icons-material";
import type { SvgIconTypeMap } from "@mui/material";
import type { OverridableComponent } from "@mui/material/OverridableComponent";
import { type ColonyParameter } from "./ColonyActions";

export interface StateItem {
    color: string,
    icon: OverridableComponent<SvgIconTypeMap<Record<string, unknown>, "svg">> & { muiName: string; },
    label: string,
    value: string | number,
    url?: string | undefined
}

export const StateItemStyleType = {
    Unknown: 0 as const,
    Solars: 1 as const,
    Laws: 2 as const,
    Population: 3 as const,
    Zones: 4 as const,
    Ship: 5 as const,
    Colony: 6 as const,
    Mood: 7 as const,
} as const;

export type StateItemStyleType = typeof StateItemStyleType[keyof typeof StateItemStyleType];

const GetBeautifulNumber = (value: number, setPlus: boolean): string => {
    if (value === 0)
        return "0";
    const absValue = Math.abs(value);
    const isNegative = value < 0;
    const simbol = isNegative ? '-' : setPlus ? '+' : '';
    if (absValue < 1) {
        const formatted = absValue.toFixed(3);
        return simbol + parseFloat(formatted).toString();
    }
    if (absValue < 1000) {
        return simbol + Math.floor(absValue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
    }

    const units = [
        { value: 1, symbol: '' },
        { value: 1e3, symbol: 'K' },      // Тысячи
        { value: 1e6, symbol: 'M' },      // Миллионы
        { value: 1e9, symbol: 'B' },      // Миллиарды
        { value: 1e12, symbol: 'T' },     // Триллионы
        { value: 1e15, symbol: 'Q' },     // Квадриллионы
        { value: 1e18, symbol: 'QT' },    // Квинтиллионы
        { value: 1e21, symbol: 'SX' },    // Секстиллионы
        { value: 1e24, symbol: 'SP' },    // Септиллионы
    ];

    let unitIndex = 0;
    for (let i = units.length - 1; i >= 0; i--) {
        if (absValue >= units[i].value) {
            unitIndex = i;
            break;
        }
    }

    const unit = units[unitIndex];
    const formattedValue = absValue / unit.value;

    let result: string;
    if (formattedValue < 100) {
        result = formattedValue.toFixed(2);
    } else {
        const fixedValue = formattedValue.toFixed(2);
        result = parseFloat(fixedValue).toString();
    }

    if (result.includes('.')) {
        result = result.replace(/,?0+$/, '');
        if (result.endsWith('.')) {
            result = result.slice(0, -1);
        }
    }

    return simbol + result + unit.symbol;
}


export const ColonyNameItemStyles = (label: string, value: string): StateItem => {
    return { color: '#9C27B0', icon: WorkspacePremium, label, value };
}

export const StateItemStyles = (stateItemStyle: StateItemStyleType, label: string, value: string, url?: string | undefined): StateItem => {
    switch (stateItemStyle) {
        case StateItemStyleType.Solars:
            return { color: '#FFD700', icon: AttachMoney, label, value, url };
        case StateItemStyleType.Population:
            return { color: '#81C784', icon: People, label, value, url };
        case StateItemStyleType.Zones:
            return { color: '#757575', icon: ViewModule, label, value, url };
        case StateItemStyleType.Ship:
            return { color: '#FF8A65', icon: RocketLaunch, label, value, url };
        case StateItemStyleType.Laws:
            return { color: '#4FC3F7', icon: Balance, label, value, url };
        case StateItemStyleType.Colony:
            return { color: '#9C27B0', icon: WorkspacePremium, label, value, url };
        case StateItemStyleType.Mood:
            return { color: '#FFC107', icon: SentimentSatisfied, label, value};
        default:
            return { color: '#000090', icon: Info, label, value, url };
    }
}

export const SolarsStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, isChanging);
    return StateItemStyles(StateItemStyleType.Solars, 'Резервы Солар', valueString);
}

export const SolarIncomeStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, true);
    const name = !isChanging
        ? 'Бюджет'
        : value < 0
            ? 'Расход'
            : 'Доход'
    return StateItemStyles(StateItemStyleType.Solars, name, valueString);
}

export const PopulationStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, isChanging);
    return StateItemStyles(StateItemStyleType.Population, 'Население', valueString);
}

export const ShipStateItem = (value: number): StateItem => {
    let stringValue = "Неопределен";
    switch (value) {
        case 1:
            stringValue = "Рассвет-782"
            break
        case 2:
            stringValue = "Резолют-206"
            break
    }
    const url = value == 0 ? undefined : `/wiki/ship/${value}}`;
    return StateItemStyles(StateItemStyleType.Ship, 'Станция', stringValue, url);
}

export const ZonesOccupiedStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, isChanging);
    return StateItemStyles(StateItemStyleType.Zones, 'Занято зон', valueString);
}

export const ZonesTotalStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, isChanging);
    return StateItemStyles(StateItemStyleType.Zones, 'Всего зон', valueString);
}

export const GetCodeOfLawsString = (value: number): string => {
    switch (Math.round(value)) {
        case 1:
            return "Гуманные";
        case 2:
            return "Стандартные";
        case 3:
            return "Корпоративные";
        default:
            return "Смешанные";
    }
}

export const MoodTypeStateItem = (value: number, isChanging: boolean): StateItem => {
    let valueString = GetBeautifulNumber(value, isChanging);
    if (!isChanging && value < 50){
        valueString += ' (риск бунта)';
    }
    return StateItemStyles(StateItemStyleType.Mood, 'Настроение', valueString);    
}

export const CodeOfLawsStateItem = (value: number): StateItem => {
    const stringValue = GetCodeOfLawsString(value);
    return {
        icon: Balance,
        label: 'Законы',
        value: stringValue,
        color: '#4FC3F7'
    }
}

const GetStateItem = (colonyParameter: ColonyParameter, isChanging: boolean): StateItem | undefined => {
    switch (colonyParameter.name) {
        case 'Economic_Reserves':
            return SolarsStateItem(colonyParameter.value, isChanging);
        case 'Economic_Budget_Balance':
            return SolarIncomeStateItem(colonyParameter.value, isChanging);
        case 'Population_Total':
            return PopulationStateItem(colonyParameter.value, isChanging);
        case 'AreaCapacity_Occupied':
            return ZonesOccupiedStateItem(colonyParameter.value, isChanging);
        case 'Mood_Total':
            return MoodTypeStateItem(colonyParameter.value, isChanging);
        case 'Laws_CodeOfLaws':
            return CodeOfLawsStateItem(colonyParameter.value);
        case "Ship_Id":
            return ShipStateItem(colonyParameter.value);
        case 'AreaCapacity_Total':
            return ZonesTotalStateItem(colonyParameter.value, isChanging);
        default:
            return undefined;
    }
}

export const GetStateItems = (colonyParameters: ColonyParameter[], isChanging: boolean): StateItem[] => {
    return colonyParameters
        .map(x => GetStateItem(x, isChanging))
        .filter(x => x != undefined);
}