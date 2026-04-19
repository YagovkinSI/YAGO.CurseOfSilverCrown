import { AccessTime, AttachMoney, Balance, GroupAdd, Info, People, RocketLaunch, SentimentSatisfied, ViewModule, WorkspacePremium } from "@mui/icons-material";
import type { SvgIconTypeMap } from "@mui/material";
import type { OverridableComponent } from "@mui/material/OverridableComponent";
import { type ColonyParameter } from "./ColonyParameter";
import type { ColonyParameterName } from "./ColonyParameterType";

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
    Attractiveness: 8 as const,
    Time: 9 as const,
} as const;

export type StateItemStyleType = typeof StateItemStyleType[keyof typeof StateItemStyleType];

const GetBeautifulNumber = (value: number, setPlus: boolean): string => {
    const isNegative = value < 0;
    const simbol = isNegative ? '-' : setPlus ? '+' : '';
    if (value === 0)
        return simbol + "0";
    const absValue = Math.abs(value);
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

export const StateItemStyles = (stateItemStyle: StateItemStyleType, label: string, value: string, url?: string | undefined): StateItem => {
    switch (stateItemStyle) {
        case StateItemStyleType.Solars:
            return { color: '#FFD700', icon: AttachMoney, label, value, url };
        case StateItemStyleType.Population:
            return { color: '#81C784', icon: People, label, value, url };
        case StateItemStyleType.Zones:
            return { color: '#757575', icon: ViewModule, label, value, url };
        case StateItemStyleType.Ship:
            return { color: '#757575', icon: RocketLaunch, label, value, url };
        case StateItemStyleType.Laws:
            return { color: '#4FC3F7', icon: Balance, label, value, url };
        case StateItemStyleType.Colony:
            return { color: '#000090', icon: WorkspacePremium, label, value, url };
        case StateItemStyleType.Mood:
            return { color: '#F57C00', icon: SentimentSatisfied, label, value, url };
        case StateItemStyleType.Attractiveness:
            return { color: '#9C27B0', icon: GroupAdd, label, value, url };
        case StateItemStyleType.Time:
            return { color: '#000090', icon: AccessTime, label, value, url };
        default:
            return { color: '#000090', icon: Info, label, value, url };
    }
}

export const ColonyNameItemStyles = (label: string, value: string): StateItem => {
    return StateItemStyles(StateItemStyleType.Colony, label, value);
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

export const AttractivenessStateItem = (value: number, isChanging: boolean): StateItem => {
    const valueString = GetBeautifulNumber(value, isChanging);
    return StateItemStyles(StateItemStyleType.Attractiveness, 'Привлекательность', valueString, '/wiki/parameters/8');
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
    if (!isChanging && value < 50) {
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

export const CurrentWeekStateItem = (value: number, isChanging: boolean): StateItem => {
    const stringValue = GetBeautifulNumber(value, isChanging);
    return {
        icon: AccessTime,
        label: 'Неделя',
        value: stringValue,
        color: '#000090'
    }
}

const GetStateItemStyleType = (colonyParameterName : ColonyParameterName) : StateItemStyleType => {
    switch (colonyParameterName) {
        case 'Colony_Name':
            return StateItemStyleType.Colony;
        case 'Economic_Reserves':
        case 'Economic_Budget_Balance':
            return StateItemStyleType.Solars;
        case 'Population_Total':
            return StateItemStyleType.Population;
        case 'AreaCapacity_Occupied':
        case 'AreaCapacity_Total':
            return StateItemStyleType.Zones;
        case 'Mood_Total':
            return StateItemStyleType.Mood;
        case 'Laws_CodeOfLaws':
            return StateItemStyleType.Laws;
        case "Ship_Id":
            return StateItemStyleType.Ship;
        case 'Attractiveness_Total':
            return StateItemStyleType.Attractiveness;
        case 'CurrentWeek':
        case 'EpisodeCount':
            return StateItemStyleType.Time;
        default:
            return StateItemStyleType.Unknown;
    }   
}

const GetStateItemUrlTemplate = (colonyParameterName : ColonyParameterName) : string | undefined => {
    switch (colonyParameterName) {
        case "Colony_Name":
            return '/state';
        case "Ship_Id":
            return '/wiki/ship/';
        case 'Attractiveness_Total':
            return '/wiki/parameters/1';
        default:
            return undefined;
    }   
}

const GetStateItem = (colonyParameter: ColonyParameter): StateItem | undefined => {
    const stateItemStyleType = GetStateItemStyleType(colonyParameter.type) 
    const stateItemUrlTemplate = GetStateItemUrlTemplate(colonyParameter.type) 
    const url = stateItemUrlTemplate == undefined
        ? undefined
        : stateItemUrlTemplate.endsWith('/')
            ? stateItemUrlTemplate + colonyParameter.url
            : stateItemUrlTemplate;
    return StateItemStyles(stateItemStyleType, colonyParameter.name, colonyParameter.value, url);
}

export const GetStateItems = (colonyParameters: ColonyParameter[]): StateItem[] => {
    return colonyParameters
        .map(x => GetStateItem(x))
        .filter(x => x != undefined);
}