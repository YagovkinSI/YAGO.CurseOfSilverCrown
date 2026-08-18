import type { ColonyEventSummary } from "../events/colonyEvent.types";

export interface ColonyPrivate {
    id: string,
    iserId: number,
    nextTurnstartAtUtc: string;
    name: string,
    colonyParameters: ColonyParameter[],
    quests: ColonyEventSummary[],
    actions: ColonyActions
}

export interface ColonyDetails {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}

export interface ColonyActions {
    reform: boolean,
    build: boolean
}

export type StatMenu = 'header' | 'stats' | 'other'; 
export type ParameterStatus = 'critical' | 'bad' | 'neutral' | 'good' | 'excellent'; 

export interface ColonyParameter {
    type: ColonyParameterName,
    statMenus?: StatMenu[],
    weight?: number,
    name: string,
    value: string,
    status?: ParameterStatus,
    url?: string | undefined
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];

export type ColonyParameterName = 
    "Colony_Name"

    //ActionPoint
    | "ActionPoints"
    | "ActionPoints_Resourses"
    | "ActionPoints_Trend"

    //Gdp
    | "Gdp"
    | "Gdp_Resourses"
    | "Gdp_Trend"

    //Economic
    | "Economic"
    | "Economic_Reserves"
    | "Economic_Budget_Balance"

    //Mood
    | "Mood_Total"

    //AreaCapacity
    | "AreaCapacity"
    | "AreaCapacity_Occupied"
    | "AreaCapacity_Total"

    //Attractiveness
    | "Attractiveness_Total"

    //Ship
    | "Ship_Id"

    //Laws
    | "Laws_CodeOfLaws"

    //Companies
    //Companies_Minning
    | "Companies_Minning_EngineeringTeam"
    | "Companies_Minning_MiningBrigade"
    | "Companies_Minning_RehabilitationContingent"

    //Population
    | "Population_Total"

    //Time
    | "CurrentWeek"

    //Other
    | "Other"

