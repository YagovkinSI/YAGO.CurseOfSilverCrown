import { apiRequester } from "../shared/ApiRequester"
import type { Faction } from "./provinces/ProvinceWithUser"

export interface AuthorizationState {
    data: AuthorizationData,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface AuthorizationData {
    isAuthorized: boolean
    user: CurrentUser | undefined,
    faction: Faction | undefined
}

export interface CurrentUser {
    id: string
    userName: string
    registration: string
    lastActivity: string
}

const defaultAuthorizationData: AuthorizationData = {
    isAuthorized: false,
    user: undefined,
    faction: undefined
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
            query: () => `/currentUser`,
            providesTags: [{ type: 'Daily', id: 'CurrentUser' }]
        }),
    })
})

export const { useGetCurrentUserQuery } = extendedApiSlice;