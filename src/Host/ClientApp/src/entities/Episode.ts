import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    prologSlides: Slide[];
    choice: Choice[];
    choiceLabel: string | undefined;
    isCycleCompleted: boolean;
}

export interface Slide {
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
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
