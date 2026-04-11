import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    prologueSlides: PrologueSlide[];
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

export interface PrologueSlide extends Slide {
    continueButtonName: string
}

export interface Dilemma {
    choice: Choice[];
    choiceType: ChoiceType;
    choiceLabel: string[];
}

export interface Choice extends Slide {
    id: string,
    isAvailable: boolean
}
