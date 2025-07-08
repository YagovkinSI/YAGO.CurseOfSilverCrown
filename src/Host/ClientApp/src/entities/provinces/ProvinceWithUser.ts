import { apiRequester } from "../../shared/ApiRequester";

export interface ProvinceWithUserState {
    data: ProvinceWithUser,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface ProvinceWithUser {
    province: Province,
    faction: Faction,
    user: User | undefined,
}

export interface Province {
    id: number
}

export interface Faction {
    id: number,
    name: string,
    gold: number,
    investments: number,
    fortifications: number,
    fortificationCoef: number,
    size: number,
    userId: string,
    suzerainId: number | undefined,
    turnOfDefeat: number
}

export interface User {
    id: number,
    userName: string,
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getProvinceWithUser: builder.query<ProvinceWithUser, number>({
            query: (params) => ({
                url: 'provinces',
                params: {
                    id: params
                }
            }),
            providesTags: [{ type: 'Daily', id: 'ProvinceWithUser' }]
        }),
    })
})

export const { useGetProvinceWithUserQuery } = extendedApiSlice;