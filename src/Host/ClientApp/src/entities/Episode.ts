import { apiRequester } from "../shared/ApiRequester";
import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
    slides: Slide[];
    dilemma: Dilemma | undefined;
}

export type DilemmaType = "Unknown" | "Select" | "TextInput"

export interface Slide {
    title: string,
    imageName: string,
    text: string[],
    parameters: ColonyParameter[],
    buttons: SlideButton[],
    continueButtonName: string,
    footer?: string | undefined
}

export interface SlideButton {
    name: string;
    isAvailable: boolean;
    action?: SlideButtonAction | undefined;
    navigate?: SlideButtonNavigate | undefined;
}

export interface SlideButtonAction {
    actionName: string;
    actionParameters: string;
}

export interface SlideButtonNavigate {
    actionUrl: string;
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

export interface EpisodeActionRequest {
    actionName: string,
    actionParameters: string
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        episodeAction: builder.mutation<Episode, EpisodeActionRequest>({
            query: (body) => ({
                url: '/episode/action',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony'],
        })
    }),
});

export const {
    useEpisodeActionMutation
} = extendedApiSlice;