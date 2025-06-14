import { apiRequester } from "../shared/ApiRequester";

export interface FactionListState {
    data: FactionListItem[],
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface FactionListItem {
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

export const defaultFactionListState: FactionListState = {
    data: [],
    isLoading: false,
    isChecked: false,
    error: ''
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getFactionList: builder.query<FactionListItem[], number | undefined>({
            query: (column) => `/factions?column=${column}`,
            providesTags: [{ type: 'Daily', id: 'FactionList' }]
        }),
    })
})

export const { useGetFactionListQuery } = extendedApiSlice;