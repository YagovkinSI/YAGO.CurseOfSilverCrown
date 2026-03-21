import type { ColonyParameterName } from "./ColonyParameterType";

export interface ColonyParameter {
    name: ColonyParameterName,
    value: number,
    isChanging: boolean
}

export interface Episode {
    id: string | undefined,
    slides: Slide[],
    choiceLabel: string | undefined,
    choice: Slide[] | undefined,
    isCycleCompleted: boolean
}

export interface Slide {
    title: string,
    illustration: string,
    text: string[],
    parameters: ColonyParameter[]
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];