import { apiRequester } from "../shared/ApiRequester";
import type YagoEnity from "./YagoEnity";

export interface FactionListState {
    data: ListData,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface ListData {
    items: ListItem[],
    count: number,
}

export interface ListItem {
    number: number | undefined,
    entity: YagoEnity,
    value: YagoEnity | undefined,
}

const defaultFactionListData : ListData = {
    items: [],
    count: 0,
}

export const defaultFactionListState: FactionListState = {
    data: defaultFactionListData,
    isLoading: false,
    isChecked: false,
    error: ''
}

export type FactionSortBy = 'name' | 'warriorCount' | 'gold' | 'investments' | 'fortifications' | 'suzerain' | 'user' | 'vassalCount';

export type ApiQueryParams = {
    sortBy: FactionSortBy;
    page: number;
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        getFactionList: builder.query<ListData, ApiQueryParams>({
            query: (params) => ({
                url: 'factions',
                params: {
                    page: params.page,
                    sortBy: params.sortBy,
                }
            }),
            providesTags: [{ type: 'Daily', id: 'FactionList' }]
        }),
    })
})

export const { useGetFactionListQuery } = extendedApiSlice;