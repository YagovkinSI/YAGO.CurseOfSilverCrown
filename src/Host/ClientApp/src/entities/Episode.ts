import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    prologueSlides: PrologueSlide[];
    dilemma: Dilemma | undefined;
    isCycleCompleted: boolean;
}

export type DilemmaType = "Unknown" | "Select" | "TextInput"

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
    dilemmaType: DilemmaType;
    choice: Choice[];
    choiceLabel: string[];
}

export interface DilemmaSelect extends Dilemma {
    dilemmaType: "Select";
}

export interface DilemmaTextInput extends Dilemma {
    dilemmaType: "TextInput";
}

export interface Choice extends Slide {
    id: string,
    isAvailable: boolean
}
