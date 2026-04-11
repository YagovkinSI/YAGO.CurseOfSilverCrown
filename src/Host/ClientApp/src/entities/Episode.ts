import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    prologSlides: Slide[];
    choice: Choice[];
    choiceType: ChoiceType;
    choiceLabel: string[];
    isCycleCompleted: boolean;
}

export type ChoiceType = "Unknown" | "Select" | "TextInput"

export interface Slide {
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    buttonName: string,
    footer?: string | undefined
}

export interface Choice {
    id: string,
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    isAvailable: boolean,
    buttonName: string,
    footer?: string | undefined
}
