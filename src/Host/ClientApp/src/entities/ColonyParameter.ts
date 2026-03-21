import type { ColonyParameterName } from "./ColonyParameterType";

export interface ColonyParameter {
    name: ColonyParameterName,
    value: number,
    isChanging: boolean
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];