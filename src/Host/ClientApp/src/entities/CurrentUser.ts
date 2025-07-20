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
            providesTags: ['CurrentUser']
        }),

        login: builder.mutation<AuthorizationData, { userName: string, password: string }>({
            query: ({ userName, password }) => ({
                url: `/currentUser/login`,
                method: 'POST',
                body: { userName, password },
            }),
            invalidatesTags: ['CurrentUser', 'CurrentStory']
        }),

        register: builder.mutation<AuthorizationData, { userName: string, password: string, passwordConfirm: string }>({
            query: ({ userName, password, passwordConfirm }) => ({
                url: `/currentUser/register`,
                method: 'POST',
                body: { userName, password, passwordConfirm },
            }),
            invalidatesTags: ['CurrentUser', 'CurrentStory']
        }),

        autoRegister: builder.mutation<AuthorizationData, void>({
            query: () => ({
                url: `/currentUser/autoRegister`,
                method: 'POST'
            }),
            invalidatesTags: ['CurrentUser', 'CurrentStory']
        }),

        changeRegistration: builder.mutation<AuthorizationData, { userName: string, password: string, passwordConfirm: string }>({
            query: ({ userName, password, passwordConfirm }) => ({
                url: `/currentUser/changeRegistration`,
                method: 'POST',
                body: { userName, password, passwordConfirm },
            }),
            invalidatesTags: ['CurrentUser', 'CurrentStory']
        }),

        logout: builder.mutation<AuthorizationData, void>({
            query: () => ({
                url: `/currentUser/logout`,
                method: 'POST'
            }),
            invalidatesTags: ['CurrentUser', 'CurrentStory']
        }),
    })
})

export const { useGetCurrentUserQuery, useLoginMutation, useRegisterMutation, useAutoRegisterMutation, useChangeRegistrationMutation, useLogoutMutation } = extendedApiSlice;