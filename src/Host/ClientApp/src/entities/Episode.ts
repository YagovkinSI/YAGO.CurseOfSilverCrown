import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    id: string | undefined;
    slides: Slide[];
    dilemma: Dilemma | undefined;
    isCycleCompleted: boolean;
}

export type DilemmaType = "Unknown" | "Select" | "TextInput"

export interface Slide {
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    continueButtonName: string,
    footer?: string | undefined
}

export interface Dilemma {
    dilemmaType: DilemmaType;
}

export interface DilemmaSelect extends Dilemma {
    dilemmaType: "Select";
    choice: Choice[];
    choiceLabel: string[];
}

export interface DilemmaTextInput extends Dilemma {
    dilemmaType: "TextInput";
    slide: Slide;
    submitButtonName: string;
}

export interface Choice extends Slide {
    id: string,
    isAvailable: boolean
}
