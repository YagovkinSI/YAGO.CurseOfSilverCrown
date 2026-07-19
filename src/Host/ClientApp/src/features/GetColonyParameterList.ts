import { Clock, Coins, Info, LayoutGrid, Medal, Rocket, Scale, Smile, TrendingUp, UserPlus, Users, Zap } from "lucide-react";
import { type ColonyParameter } from "../entities/colonies/ColonyParameter";
import type { ColonyParameterName } from "../entities/colonies/ColonyParameterType";
import type { ColonyParameterRowProps } from "../entities/colonies/ColonyParameterRow";

export const GetParameterIcon = (colonyParameterName: ColonyParameterName) : React.ElementType => {
    switch (colonyParameterName) {
        case 'Colony_Name':
            return Medal;
        case 'ActionPoints':
        case 'ActionPoints_Resourses':
        case 'ActionPoints_Trend':
            return Zap;
        case 'Gdp':
        case 'Gdp_Resourses':
        case 'Gdp_Trend':
            return TrendingUp;
        case 'Economic':
        case 'Economic_Reserves':
        case 'Economic_Budget_Balance':
            return Coins;
        case 'Population_Total':
            return Users;
        case 'AreaCapacity':
        case 'AreaCapacity_Occupied':
        case 'AreaCapacity_Total':
            return LayoutGrid;
        case "Ship_Id":
            return Rocket;
        case 'Laws_CodeOfLaws':
            return Scale;
        case 'Mood_Total':
            return Smile;
        case 'Attractiveness_Total':
            return UserPlus;
        case 'CurrentWeek':
            return Clock;
        default:
            return Info;
    }
}

const StateItemStyles = (colonyParameterName: ColonyParameterName, label: string, value: string, url?: string | undefined): ColonyParameterRowProps => {
    const icon = GetParameterIcon(colonyParameterName);
    return { icon, label, value, url };
}

const GetStateItemUrlTemplate = (colonyParameterName: ColonyParameterName): string | undefined => {
    switch (colonyParameterName) {
        case "Other":
            return '/me/statistics/other';
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
    var colonyParameterRowProps = StateItemStyles(colonyParameter.type, colonyParameter.name, colonyParameter.value, url);
    colonyParameterRowProps.status = colonyParameter.status;
    return colonyParameterRowProps;
}

export const GetStateItems = (colonyParameters: ColonyParameter[]): ColonyParameterRowProps[] => {
    return colonyParameters
        .map(x => GetStateItem(x))
        .filter(x => x != undefined);
};