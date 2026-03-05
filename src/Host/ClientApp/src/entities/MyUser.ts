import { apiRequester } from "../shared/ApiRequester"
import { createMyDataMutation } from "./ApiResponse";
import type { MyDataResponse } from './MyDataResponse';

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
        get: builder.query<MyDataResponse<MyUser>, void>({
            query: () => 'me/user/get',
            providesTags: ['MyUser'],
        }),

        login: createMyDataMutation<MyDataResponse<MyUser>, {userName: string; password: string; }>(
            '/me/user/login', builder, ['MyUser', 'MyColony', 'MyCycle']),

        register: createMyDataMutation<MyDataResponse<MyUser>, {userName: string; password: string; passwordConfirm: string; }>(
            '/me/user/register', builder, ['MyUser', 'MyColony', 'MyCycle']),

        logout: createMyDataMutation<MyDataResponse<MyUser>, {}>(
            '/me/user/logout', builder, ['MyUser', 'MyColony', 'MyCycle']),

        createTemporaryUser: createMyDataMutation<MyDataResponse<MyUser>, {}>(
            '/me/user/createTemporaryUser', builder, ['MyUser', 'MyColony', 'MyCycle']),

        convertToPermanentUser: createMyDataMutation<MyDataResponse<MyUser>, { userName: string; password: string; passwordConfirm: string; }>(
            '/me/user/convertToPermanentUser', builder, ['MyUser', 'MyColony', 'MyCycle']),
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