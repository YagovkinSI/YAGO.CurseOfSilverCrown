import { apiRequester } from "../shared/ApiRequester";
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
    buttons: SlideButton[],
    continueButtonName: string,
    textInput?: TextInput | undefined,  
    footer?: string | undefined
}

export interface SlideButton {
    name: string;
    isAvailable: boolean;
    action?: SlideButtonAction | undefined;
    navigate?: SlideButtonNavigate | undefined;
    toSlide?: SlideButtonToSlide | undefined;
}

export interface SlideButtonAction {
    actionName: string;
    actionParameters: string;
}

export interface SlideButtonNavigate {
    actionUrl: string;
}

export interface SlideButtonToSlide {
    slideId: string;
}

export interface TextInput {
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