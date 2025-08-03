import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

export interface AuthorizationState {
    data: AuthorizationData,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface AuthorizationData {
    isAuthorized: boolean
    user: UserPrivate | undefined,
}

export interface UserPrivate {
    id: string
    userName: string
    email: string | undefined
    registered: string
    lastActivity: string
}

const createCurrentUserMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<AuthorizationData, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getAuthorizationData', undefined, data)
            );
        },
        invalidatesTags: ['CurrentChapter', 'StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getAuthorizationData: builder.query<AuthorizationData, void>({
            query: () => 'authorization/getAuthorizationData',
            providesTags: ['AuthorizationData'],
        }),

        login: createCurrentUserMutation<{
            userName: string;
            password: string;
        }>('/authorization/login', builder),

        register: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/authorization/register', builder),

        autoRegister: createCurrentUserMutation('/authorization/autoRegister', builder),

        changeRegistration: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/authorization/changeRegistration', builder),

        logout: createCurrentUserMutation('/authorization/logout', builder),
    }),
});


export const {
    useGetAuthorizationDataQuery,
    useLoginMutation,
    useRegisterMutation,
    useAutoRegisterMutation,
    useChangeRegistrationMutation,
    useLogoutMutation,
} = extendedApiSlice;