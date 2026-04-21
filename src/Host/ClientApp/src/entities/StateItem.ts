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

export const StateItemStyles = (colonyParameterName: ColonyParameterName, label: string, value: string, url?: string | undefined): StateItem => {
    switch (colonyParameterName) {
        case 'Colony_Name':
            return { color: '#000090', icon: WorkspacePremium, label, value, url };
        case 'Economic':
        case 'Economic_Reserves':
        case 'Economic_Budget_Balance':
            return { color: '#FFD700', icon: AttachMoney, label, value, url };
        case 'Population_Total':
            return { color: '#81C784', icon: People, label, value, url };
        case 'AreaCapacity':
        case 'AreaCapacity_Occupied':
        case 'AreaCapacity_Total':
            return { color: '#757575', icon: ViewModule, label, value, url };
        case "Ship_Id":
            return { color: '#757575', icon: RocketLaunch, label, value, url };
        case 'Laws_CodeOfLaws':
            return { color: '#4FC3F7', icon: Balance, label, value, url };
        case 'Mood_Total':
            return { color: '#F57C00', icon: SentimentSatisfied, label, value, url };
        case 'Attractiveness_Total':
            return { color: '#9C27B0', icon: GroupAdd, label, value, url };
        case 'CurrentWeek':
        case 'EpisodeCount':
            return { color: '#000090', icon: AccessTime, label, value, url };
        default:
            return { color: '#000090', icon: Info, label, value, url };
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
    const stateItemUrlTemplate = GetStateItemUrlTemplate(colonyParameter.type) 
    const url = stateItemUrlTemplate == undefined
        ? undefined
        : stateItemUrlTemplate.endsWith('/')
            ? stateItemUrlTemplate + colonyParameter.url
            : stateItemUrlTemplate;
    return StateItemStyles(colonyParameter.type, colonyParameter.name, colonyParameter.value, url);
}

export const GetStateItems = (colonyParameters: ColonyParameter[]): StateItem[] => {
    return colonyParameters
        .map(x => GetStateItem(x))
        .filter(x => x != undefined);
}