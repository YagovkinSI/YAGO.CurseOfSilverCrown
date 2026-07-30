import { apiRequester } from "../../shared/api/ApiRequester"
import type { ApiResponse } from '../../shared/api/ApiResponse';

export interface MyUser {
    id: string
    userName: string
    email: string | undefined
    registered: string
    lastActivity: string,
    isTemporary: boolean
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getMyUser: builder.query<ApiResponse<MyUser>, void>({
            query: () => '/me/user/getMyUser',
            providesTags: ['MyUser'],
        }),

        login: builder.mutation<void, { userName: string; password: string; }>({
            query: (body) => ({
                url: '/me/user/login',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyUser', 'MyColony', 'MyCycle', 'MyBuildings'],
        }),

        register: builder.mutation<void, { userName: string; password: string; passwordConfirm: string; }>({
            query: (body) => ({
                url: '/me/user/register',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyUser', 'MyColony', 'MyCycle', 'MyBuildings'],
        }),

        logout: builder.mutation<void, void>({
            query: (body) => ({
                url: '/me/user/logout',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyUser', 'MyColony', 'MyCycle', 'MyBuildings'],
        }),

        createTemporaryUser: builder.mutation<void, void>({
            query: (body) => ({
                url: '/me/user/createTemporaryUser',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyUser', 'MyColony', 'MyCycle', 'MyBuildings'],
        }),

        convertToPermanentUser: builder.mutation<void, { userName: string; password: string; passwordConfirm: string; }>({
            query: (body) => ({
                url: '/me/user/convertToPermanentUser',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyUser'],
        })
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