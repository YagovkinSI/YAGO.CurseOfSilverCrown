import { apiRequester } from "../../shared/api/ApiRequester";
import type { ColonyParameter } from "./ColonyParameter";
import type { PaginatedResponse } from "../../shared/api/PaginatedResponse";

export interface ColonyDetails {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyRaiting: builder.query<PaginatedResponse<ColonyDetails>, { page: number }>({
            query: ({ page }) => `colonies/getColonyRaiting?page=${page}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
    }),
});


export const {
    useGetColonyRaitingQuery,
} = extendedApiSlice;