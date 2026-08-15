import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { UserPrivate } from "./user.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getUserPrivate: builder.query<ApiResponse<UserPrivate>, void>({
            query: () => '/users/getUserPrivate',
            providesTags: ['UserPrivate'],
        }),

        login: builder.mutation<void, { userName: string; password: string; }>({
            query: (body) => ({
                url: '/users/login',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['UserPrivate', 'MyColony', 'MyBuildings'],
        }),

        register: builder.mutation<void, { userName: string; password: string; passwordConfirm: string; }>({
            query: (body) => ({
                url: '/users/register',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['UserPrivate', 'MyColony', 'MyBuildings'],
        }),

        logout: builder.mutation<void, void>({
            query: (body) => ({
                url: '/users/logout',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['UserPrivate', 'MyColony', 'MyBuildings'],
        }),

        createTemporaryUser: builder.mutation<void, void>({
            query: (body) => ({
                url: '/users/createTemporaryUser',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['UserPrivate', 'MyColony', 'MyBuildings'],
        }),

        convertToPermanentUser: builder.mutation<void, { userName: string; password: string; passwordConfirm: string; }>({
            query: (body) => ({
                url: '/users/convertToPermanentUser',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['UserPrivate'],
        })
    }),
});

export const {
    useGetUserPrivateQuery, 
    useLoginMutation, 
    useRegisterMutation, 
    useCreateTemporaryUserMutation, 
    useConvertToPermanentUserMutation, 
    useLogoutMutation,
} = extendedApiSlice;