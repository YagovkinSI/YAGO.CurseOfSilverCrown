import type { ColonyParameter } from "../colonies/colony.types";
import type { GameRequirement } from "../common/gameRequirements/gameRequirement.types";
import type { GameVisibleEffect } from "../common/gameVisibleEffects/gameVisibleEffect.types";

export type EventType = 'Default' | 'Autostart' | 'Urgent' | 'Quest';

export interface ColonyEventPrivate {
    id: number,
    title: string,
    type: EventType,
    episode: Episode,
    isRead: boolean,
    createdAtUtc: string,
    turnsLeft: number
}

export interface ColonyEventSummary {
    id: number,
    title: string,
    type: EventType,
    isRead: boolean,
    createdAtUtc: string,
    turnsLeft: number
}

export interface Episode {
    slides: Slide[]
}

export type DilemmaType = "Unknown" | "Select" | "TextInput"

export interface Slide {
    id: string,
    title: string,
    imageName: string,
    text: string[],
    visibleEffects: GameVisibleEffect[],
    requirements: GameRequirement[],
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

export interface EventResultSlide {
    title: string,
    text: string[],
    parameters: ColonyParameter[],
    imageName?: string,
}
