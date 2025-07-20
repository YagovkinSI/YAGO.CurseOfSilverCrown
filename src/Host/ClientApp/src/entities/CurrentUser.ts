import { apiRequester } from "../shared/ApiRequester"

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

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getCurrentUser: builder.query<AuthorizationData, void>({
            query: () => `/currentUser/getCurrentUser`,
            providesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),

        login: builder.mutation<AuthorizationData, { userName: string, password: string }>({
            query: ({ userName, password }) => ({
                url: `/currentUser/login`,
                method: 'POST',
                body: { userName, password },
            }),
            invalidatesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),

        register: builder.mutation<AuthorizationData, { userName: string, password: string, passwordConfirm: string }>({
            query: ({ userName, password, passwordConfirm }) => ({
                url: `/currentUser/register`,
                method: 'POST',
                body: { userName, password, passwordConfirm },
            }),
            invalidatesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),

        autoRegister: builder.mutation<AuthorizationData, void>({
            query: () => ({
                url: `/currentUser/autoRegister`,
                method: 'POST'
            }),
            invalidatesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),

        logout: builder.mutation<AuthorizationData, void>({
            query: () => ({
                url: `/currentUser/logout`,
                method: 'POST'
            }),
            invalidatesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),
    })
})

export const { useGetCurrentUserQuery, useLoginMutation, useRegisterMutation, useAutoRegisterMutation, useLogoutMutation } = extendedApiSlice;