import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';
import type { ApiResponse } from './ApiResponse';

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
    return builder.mutation<ApiResponse<MyUser>, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getMyUser', undefined, data)
            );
        },
        invalidatesTags: ['MyUser', 'MyColony', 'MyCycle']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyUser: builder.query<ApiResponse<MyUser>, void>({
            query: () => 'me/user/get',
            providesTags: ['MyUser'],
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
    useGetMyUserQuery,
    useLoginMutation,
    useRegisterMutation,
    useCreateTemporaryUserMutation,
    useConvertToPermanentUserMutation,
    useLogoutMutation,
} = extendedApiSlice;