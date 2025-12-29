import { apiRequester } from "../shared/ApiRequester";
import type { PaginatedResponse } from "./PaginatedResponse";

export interface ColonyDetails {
    id: number,
    iserId: number,
    name: string,
    solarsIncome: number,
    challenges: number,
    population: number,
    zonesOccupied: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyRaiting: builder.query<PaginatedResponse<ColonyDetails>, { page: number }>({
            query: ({ page }) => `colonies/getColonyRaiting?page=${page}`            
        }),

        getColonyDetails: builder.query<ColonyDetails, { colonyId: number }>({
            query: ({ colonyId }) => `colonies/getColonyDetails?colonyId=${colonyId}`            
        }),
    }),
});


export const {
    useGetColonyRaitingQuery,
    useGetColonyDetailsQuery
} = extendedApiSlice;