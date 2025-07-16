import { apiRequester } from "../shared/ApiRequester"

export interface StoryNodeState {
    data: StoryNode,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface StoryNode {
    id: number,
    title: string,
    cards: StoryCard[],
    choices: StoryChoice[]
}

export interface StoryCard {
    number: number
    text: string
    imageName: string
}

export interface StoryChoice {
    number: number
    text: string
}

const defaultStoryNode: StoryNode = {
    id: -1,
    title: "Ошибка получения данных",
    cards: [],
    choices: []
}

export const defaultStoryNodeState: StoryNodeState = {
    data: defaultStoryNode,
    isLoading: false,
    isChecked: false,
    error: ''
}

export type SetChoiceParams = {
    storyNodeId: number;
    choiceNumber: number;
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getCurrentStory: builder.query<StoryNode, void>({
            query: () => `/story`,
            providesTags: [{ type: 'Daily', id: 'Story' }]
        }),

        setChoice: builder.mutation<StoryNode, SetChoiceParams>({
            query: (params) => ({
                url: '/story/SetChoice',
                method: 'POST',
                body: params,
                headers: {
                    'Content-Type': 'application/json'
                }
            }),
            invalidatesTags: [{ type: 'Daily', id: 'Story' }]
        }),
    })
})

export const { useGetCurrentStoryQuery, useSetChoiceMutation } = extendedApiSlice;