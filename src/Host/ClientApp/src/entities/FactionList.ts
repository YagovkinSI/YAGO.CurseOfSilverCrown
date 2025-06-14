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

export interface PaginationParams {
    page: number;
    pageSize: number;
}

export type FactionSortBy = 'name' | 'warriorCount' | 'gold' | 'investments' | 'fortifications' | 'suzerain' | 'user' | 'vassalCount';

export interface SortParams {
    sortBy: FactionSortBy;
    sortOrder: 'asc' | 'desc';
}

export type ApiQueryParams = PaginationParams & Partial<SortParams>;

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getFactionList: builder.query<FactionListItem[], ApiQueryParams>({
            query: (params) => ({
                url: 'factions',
                params: {
                    page: params.page,
                    pageSize: params.pageSize,
                    sortBy: params.sortBy,
                    sortOrder: params.sortOrder
                }
            }),
            providesTags: [{ type: 'Daily', id: 'FactionList' }]
        }),
    })
})

export const { useGetFactionListQuery } = extendedApiSlice;