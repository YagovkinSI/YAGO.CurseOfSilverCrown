import { apiRequester } from "../shared/ApiRequester";
import type { PaginatedResponse } from "./PaginatedResponse";

export interface ColonyDetails {
    id: number,
    iserId: number,
    name: string,
    solarsIncome: number,
    gavernorType: number,
    population: number,
    zonesOccupied: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyRaiting: builder.query<PaginatedResponse<ColonyDetails>, { page: number }>({
            query: ({ page }) => `colonies/getColonyRaiting?page=${page}`            
        }),
    }),
});


export const {
    useGetColonyRaitingQuery,
} = extendedApiSlice;