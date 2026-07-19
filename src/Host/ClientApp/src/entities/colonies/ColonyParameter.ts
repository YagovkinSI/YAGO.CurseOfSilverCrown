import type { ColonyParameterName } from "./ColonyParameterType";

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