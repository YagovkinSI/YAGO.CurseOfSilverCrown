import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    prologSlides: Slide[];
    dilemma: Dilemma | undefined;
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

export interface Dilemma {
    choice: Choice[];
    choiceType: ChoiceType;
    choiceLabel: string[];
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
