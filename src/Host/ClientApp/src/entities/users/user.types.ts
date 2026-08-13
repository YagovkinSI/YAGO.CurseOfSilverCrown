export interface UserPrivate {
    id: string
    userName: string
    email: string | undefined
    registered: string
    lastActivity: string,
    isTemporary: boolean
}