import { apiRequester } from "../shared/ApiRequester";
import type YagoEnity from "./YagoEnity";

export interface FactionListState {
    data: ListItem[],
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface ListItem {
    number: number | undefined,
    entity: YagoEnity,
    value: YagoEnity | undefined,
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

        getFactionList: builder.query<ListItem[], ApiQueryParams>({
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