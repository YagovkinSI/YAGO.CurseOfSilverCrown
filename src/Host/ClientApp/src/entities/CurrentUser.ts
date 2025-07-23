import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

export interface AuthorizationState {
    data: AuthorizationData,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface AuthorizationData {
    isAuthorized: boolean
    user: CurrentUser | undefined,
}

export interface CurrentUser {
    id: string
    userName: string
    email: string | undefined
    registered: string
    lastActivity: string
}

const defaultAuthorizationData: AuthorizationData = {
    isAuthorized: false,
    user: undefined,
}

export const defaultAuthorizationState: AuthorizationState = {
    data: defaultAuthorizationData,
    isLoading: false,
    isChecked: false,
    error: ''
}

const createCurrentUserMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, "CurrentUser" | "CurrentStory", "apiRequester">
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
                extendedApiSlice.util.upsertQueryData('getCurrentUser', undefined, data)
            );
        },
        invalidatesTags: ['CurrentStory']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getCurrentUser: builder.query<AuthorizationData, void>({
            query: () => 'currentUser/getCurrentUser',
            providesTags: ['CurrentUser'],
        }),

        login: createCurrentUserMutation<{
            userName: string;
            password: string;
        }>('/currentUser/login', builder),

        register: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/currentUser/register', builder),

        autoRegister: createCurrentUserMutation('/currentUser/autoRegister', builder),

        changeRegistration: createCurrentUserMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/currentUser/changeRegistration', builder),

        logout: createCurrentUserMutation('/currentUser/logout', builder),
    }),
});


export const {
    useGetCurrentUserQuery,
    useLoginMutation,
    useRegisterMutation,
    useAutoRegisterMutation,
    useChangeRegistrationMutation,
    useLogoutMutation,
} = extendedApiSlice;