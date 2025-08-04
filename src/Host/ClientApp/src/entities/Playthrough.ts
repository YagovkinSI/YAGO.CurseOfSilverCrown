import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

export interface PlaythroughState {
    data: Playthrough,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface Playthrough {
    gameSessionId: number,
    currentFragmentId: number,
    chapterNumber: number,
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
    return builder.mutation<Playthrough, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getPlaythrough', undefined, data)
            );
        },
        invalidatesTags:['StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getPlaythrough: builder.query<Playthrough, void>({
            query: () => 'playthrough/getPlaythrough',
            providesTags: ['Playthrough'],
        }),

        setChoice: createCurrentStoryMutation<{
            storyNodeId: number;
            choiceNumber: number;
        }>('/playthrough/setChoice', builder),

        dropPlaythrough: createCurrentStoryMutation('/playthrough/dropPlaythrough', builder),
    }),
});


export const {
    useGetPlaythroughQuery,
    useSetChoiceMutation,
    useDropPlaythroughMutation
} = extendedApiSlice;