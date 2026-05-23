import { apiRequester } from "../shared/ApiRequester";
import type { ColonyParameter } from "./ColonyParameter";

export interface Episode {
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

export interface EpisodeActionRequest {
    actionName: string,
    actionParameters: string
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        action: builder.mutation<Episode, EpisodeActionRequest>({
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
    useActionMutation
} = extendedApiSlice;