import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    slides: Slide[]
}

export type DilemmaType = "Unknown" | "Select" | "TextInput"

export interface Slide {
    id: string,
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    requirements: ColonyParameter[],
    buttons: SlideButton[],
    textInput?: TextInput | undefined,  
    footer?: string | undefined
}

export interface SlideButton {
    name: string;
    isAvailable: boolean;
    action?: SlideButtonAction | undefined;
    navigate?: SlideButtonNavigate | undefined;
    toSlide?: SlideButtonToSlide | undefined;
    infoSlideId?: string;
}

export interface SlideButtonAction {
    type: 'default' | 'inputCompleted' | 'inputMissed';
    actionName: string;
    arguments: string[];
}

export interface SlideButtonNavigate {
    actionUrl: string;
}

export interface SlideButtonToSlide {
    slideId: string;
}

export interface TextInput {
    preload: string
}

export interface Choice extends Slide {
    id: string,
    isAvailable: boolean
}

export interface EpisodeActionRequest {
    actionName: string,
    actionParameters: string
}