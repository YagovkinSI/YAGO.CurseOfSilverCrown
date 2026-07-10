import { Clock, Coins, Info, LayoutGrid, Medal, Rocket, Scale, Smile, TrendingUp, UserPlus, Users, Zap } from "lucide-react";
import { type ColonyParameter } from "../entities/ColonyParameter";
import type { ColonyParameterName } from "../entities/ColonyParameterType";
import type { ColonyParameterRowProps } from "../shared/ColonyParameterRow";

const StateItemStyles = (colonyParameterName: ColonyParameterName, label: string, value: string, url?: string | undefined): ColonyParameterRowProps => {
    switch (colonyParameterName) {
        case 'Colony_Name':
            return { color: '#000090', icon: Medal, label, value, url };
        case 'ActionPoints':
        case 'ActionPoints_Resourses':
        case 'ActionPoints_Trend':
            return { color: '#40E0D0', icon: Zap, label, value, url };
        case 'Gdp':
        case 'Gdp_Resourses':
        case 'Gdp_Trend':
            return { color: '#9C27B0', icon: TrendingUp, label, value, url };
        case 'Economic':
        case 'Economic_Reserves':
        case 'Economic_Budget_Balance':
            return { color: '#FFD700', icon: Coins, label, value, url };
        case 'Population_Total':
            return { color: '#81C784', icon: Users, label, value, url };
        case 'AreaCapacity':
        case 'AreaCapacity_Occupied':
        case 'AreaCapacity_Total':
            return { color: '#757575', icon: LayoutGrid, label, value, url };
        case "Ship_Id":
            return { color: '#757575', icon: Rocket, label, value, url };
        case 'Laws_CodeOfLaws':
            return { color: '#4FC3F7', icon: Scale, label, value, url };
        case 'Mood_Total':
            return { color: '#F57C00', icon: Smile, label, value, url };
        case 'Attractiveness_Total':
            return { color: '#9C27B0', icon: UserPlus, label, value, url };
        case 'CurrentWeek':
            return { color: '#000090', icon: Clock, label, value, url };
        default:
            return { color: '#000090', icon: Info, label, value, url };
    }
}

const GetStateItemUrlTemplate = (colonyParameterName: ColonyParameterName): string | undefined => {
    switch (colonyParameterName) {
        case "Other":
            return '/me/statistics';
        case "Ship_Id":
            return '/wiki/ship/';
        case 'Attractiveness_Total':
            return '/wiki/parameters/8';
        default:
            return undefined;
    }
}

const GetStateItem = (colonyParameter: ColonyParameter): ColonyParameterRowProps | undefined => {
    const stateItemUrlTemplate = GetStateItemUrlTemplate(colonyParameter.type)
    const url = stateItemUrlTemplate == undefined
        ? undefined
        : stateItemUrlTemplate.endsWith('/')
            ? stateItemUrlTemplate + colonyParameter.url
            : stateItemUrlTemplate;
    return StateItemStyles(colonyParameter.type, colonyParameter.name, colonyParameter.value, url);
}

export const GetStateItems = (colonyParameters: ColonyParameter[]): ColonyParameterRowProps[] => {
    return colonyParameters
        .map(x => GetStateItem(x))
        .filter(x => x != undefined);
};