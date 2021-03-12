import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';
import type { MyDataResponse } from './MyDataResponse';

export interface MyUserState {
    data: MyDataResponse<MyUser>,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface MyUser {
    id: string
    userName: string
    email: string | undefined
    registered: string
    lastActivity: string,
    isTemporary: boolean
}

const createMyDataMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<MyDataResponse<MyUser>, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('get', undefined, data)
            );
        },
        invalidatesTags: ['Playthrough', 'StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        get: builder.query<MyDataResponse<MyUser>, void>({
            query: () => 'me/user/get',
            providesTags: ['AuthorizationData'],
        }),

        login: createMyDataMutation<{
            userName: string;
            password: string;
        }>('/me/user/login', builder),

        register: createMyDataMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/me/user/register', builder),

        logout: createMyDataMutation('/me/user/logout', builder),

        createTemporaryUser: createMyDataMutation('/me/user/createTemporaryUser', builder),

        convertToPermanentUser: createMyDataMutation<{
            userName: string;
            password: string;
            passwordConfirm: string;
        }>('/me/user/convertToPermanentUser', builder),
    }),
});


export const {
    useGetQuery,
    useLoginMutation,
    useRegisterMutation,
    useCreateTemporaryUserMutation,
    useConvertToPermanentUserMutation,
    useLogoutMutation,
} = extendedApiSlice;