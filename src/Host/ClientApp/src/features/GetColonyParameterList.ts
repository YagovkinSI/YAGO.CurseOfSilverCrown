import { Clock, Coins, Info, LayoutGrid, Medal, Rocket, Scale, Smile, TrendingUp, UserPlus, Users, Zap } from "lucide-react";
import { type ColonyParameter } from "../entities/ColonyParameter";
import type { ColonyParameterName } from "../entities/ColonyParameterType";
import type { ColonyParameterRowProps } from "../shared/ColonyParameterRow";

const StateItemStyles = (colonyParameterName: ColonyParameterName, label: string, value: string, url?: string | undefined): ColonyParameterRowProps => {
    switch (colonyParameterName) {
        case 'Colony_Name':
            return { icon: Medal, label, value, url };
        case 'ActionPoints':
        case 'ActionPoints_Resourses':
        case 'ActionPoints_Trend':
            return { icon: Zap, label, value, url };
        case 'Gdp':
        case 'Gdp_Resourses':
        case 'Gdp_Trend':
            return { icon: TrendingUp, label, value, url };
        case 'Economic':
        case 'Economic_Reserves':
        case 'Economic_Budget_Balance':
            return { icon: Coins, label, value, url };
        case 'Population_Total':
            return { icon: Users, label, value, url };
        case 'AreaCapacity':
        case 'AreaCapacity_Occupied':
        case 'AreaCapacity_Total':
            return { icon: LayoutGrid, label, value, url };
        case "Ship_Id":
            return { icon: Rocket, label, value, url };
        case 'Laws_CodeOfLaws':
            return { icon: Scale, label, value, url };
        case 'Mood_Total':
            return { icon: Smile, label, value, url };
        case 'Attractiveness_Total':
            return { icon: UserPlus, label, value, url };
        case 'CurrentWeek':
            return { icon: Clock, label, value, url };
        default:
            return { icon: Info, label, value, url };
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