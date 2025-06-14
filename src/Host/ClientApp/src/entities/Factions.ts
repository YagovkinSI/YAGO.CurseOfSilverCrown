import { apiRequester } from "../shared/ApiRequester";

export interface FactionDataState {
    data: Faction[],
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface Faction {
    id: number,
    name: string,
    warriors: number,
    gold: number,
    investments: number,
    fortificationCoef: number,
    suzerain: string | undefined,
    vassalsCount: number,
    user: string | undefined,
}

export const defaultFactionDataState: FactionDataState = {
    data: [],
    isLoading: false,
    isChecked: false,
    error: ''
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getFactions: builder.query<Faction[], number | undefined>({
            query: (column) => `/factions?column=${column}`,
            providesTags: [{ type: 'Daily', id: 'Map' }]
        }),
    })
})

export const { useGetFactionsQuery } = extendedApiSlice;