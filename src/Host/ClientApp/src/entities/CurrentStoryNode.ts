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
    slides: Slide[],
    currentSlideIndex: number,
    choices: StoryChoice[]
}

export interface Slide {
    id: number
    text: string[]
    imageName: string
}

export interface StoryChoice {
    fragmentId: number
    text: string
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
                extendedApiSlice.util.upsertQueryData('getCurrentFragment', undefined, data)
            );
        },
        invalidatesTags:['StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getCurrentFragment: builder.query<StoryNode, void>({
            query: () => 'story/getCurrentFragment',
            providesTags: ['CurrentChapter'],
        }),

        setChoice: createCurrentStoryMutation<{
            storyNodeId: number;
            choiceNumber: number;
        }>('/story/SetChoice', builder),

        dropStory: createCurrentStoryMutation('/story/dropStory', builder),
    }),
});


export const {
    useGetCurrentFragmentQuery,
    useSetChoiceMutation,
    useDropStoryMutation
} = extendedApiSlice;