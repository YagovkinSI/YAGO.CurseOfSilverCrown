import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { AuthorizationData } from './CurrentUser';
import type { StoryNode } from './StoryNode';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester } from '../shared/ApiRequester';

export type ApiBaseQuery = BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
>;

export type ApiEndpoints = {

    getCurrentUser: {
        args: void;
        result: AuthorizationData
    };

    getCurrentStory: {
        args: void;
        result: StoryNode
    };

};

type ApiMeta = {
  cacheControl?: string;
  metrics?: {
    duration: number;
  };
};

export const createCurrentUserMutation = <BodyType extends Record<string, unknown>>(
    url: string
) => {
    return (
        builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, "CurrentUser" | "CurrentStory", "apiRequester">
    ) => {
        return builder.mutation<ApiEndpoints['getCurrentUser']['result'], BodyType>({
            query: (body) => ({
                url,
                method: 'POST',
                body,
            }),
            async onQueryStarted(_, { dispatch, queryFulfilled }) {
                const { data } = await queryFulfilled;
                dispatch(
                    extendedApiSlice.util.upsertQueryData('getCurrentUser', undefined, data)
                );
            },
            invalidatesTags: ['CurrentStory']
        });
    };
};

export const createCurrentStoryMutation = <BodyType extends Record<string, unknown>>(
    url: string
) => {
    return (
        builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, "CurrentUser" | "CurrentStory", "apiRequester">
    ) => {
        return builder.mutation<ApiEndpoints['getCurrentStory']['result'], BodyType>({
            query: (body) => ({
                url,
                method: 'POST',
                body,
            }),
            async onQueryStarted(_, { dispatch, queryFulfilled }) {
                const { data } = await queryFulfilled;
                dispatch(
                    extendedApiSlice.util.upsertQueryData('getCurrentStory', undefined, data)
                );
            },
        });
    };
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getCurrentUser: builder.query<AuthorizationData, void>({
            query: () => 'currentUser/getCurrentUser',
            providesTags: ['CurrentUser'],
        }),

        getCurrentStory: builder.query<StoryNode, void>({
            query: () => 'story/getCurrentStoryNode',
            providesTags: ['CurrentStory'],
        }),

        login: createCurrentUserMutation<{
            userName: string;
            password: string;
        }>('/currentUser/login')(builder),

        register: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/currentUser/register')(builder),

        autoRegister: createCurrentUserMutation('/currentUser/autoRegister')(builder),

        changeRegistration: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/currentUser/changeRegistration')(builder),

        logout: createCurrentUserMutation('/currentUser/logout')(builder),

        setChoice: createCurrentStoryMutation<{
            storyNodeId: number;
            choiceNumber: number;
        }>('/story/SetChoice')(builder),

        dropStory: createCurrentStoryMutation('/story/dropStory')(builder),
    }),
});


export const {
    useGetCurrentUserQuery,
    useLoginMutation,
    useRegisterMutation,
    useAutoRegisterMutation,
    useChangeRegistrationMutation,
    useLogoutMutation,
    useGetCurrentStoryQuery,
    useSetChoiceMutation,
    useDropStoryMutation
} = extendedApiSlice;