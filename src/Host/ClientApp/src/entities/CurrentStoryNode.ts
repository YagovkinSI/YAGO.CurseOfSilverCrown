import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

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
    text: string[]
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

export const createCurrentStoryMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<StoryNode, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getCurrentStoryNode', undefined, data)
            );
        },
        invalidatesTags:['StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getCurrentStoryNode: builder.query<StoryNode, void>({
            query: () => 'story/getCurrentStoryNode',
            providesTags: ['CurrentStory'],
        }),

        setChoice: createCurrentStoryMutation<{
            storyNodeId: number;
            choiceNumber: number;
        }>('/story/SetChoice', builder),

        dropStory: createCurrentStoryMutation('/story/dropStory', builder),
    }),
});


export const {
    useGetCurrentStoryNodeQuery,
    useSetChoiceMutation,
    useDropStoryMutation
} = extendedApiSlice;