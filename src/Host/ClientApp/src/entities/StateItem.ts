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
            return '/wiki/parameters/8';
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