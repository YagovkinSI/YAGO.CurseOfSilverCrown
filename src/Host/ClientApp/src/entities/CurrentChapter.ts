import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

export interface CurrentChapterState {
    data: CurrentChapter,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface CurrentChapter {
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
    return builder.mutation<CurrentChapter, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getCurrentChapter', undefined, data)
            );
        },
        invalidatesTags:['StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getCurrentChapter: builder.query<CurrentChapter, void>({
            query: () => 'story/getCurrentChapter',
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
    useGetCurrentChapterQuery,
    useSetChoiceMutation,
    useDropStoryMutation
} = extendedApiSlice;