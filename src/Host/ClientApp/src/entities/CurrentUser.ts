import type { StoryNode } from "./StoryNode"

export interface AuthorizationState {
    data: AuthorizationData,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface AuthorizationData {
    isAuthorized: boolean
    user: CurrentUser | undefined,
    storyNode: StoryNode | undefined
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
    storyNode: undefined
}

export const defaultAuthorizationState: AuthorizationState = {
    data: defaultAuthorizationData,
    isLoading: false,
    isChecked: false,
    error: ''
}