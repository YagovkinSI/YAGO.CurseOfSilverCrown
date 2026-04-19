import type { ColonyParameterName } from "./ColonyParameterType";

export interface ColonyParameter {
    type: ColonyParameterName,
    parrentType: ColonyParameterName | undefined,
    weight: number,
    name: string,
    value: string,
    url: string | undefined
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];