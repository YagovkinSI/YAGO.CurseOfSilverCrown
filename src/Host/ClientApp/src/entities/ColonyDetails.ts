import { apiRequester } from "../shared/ApiRequester";
import type { ColonyParameter } from "./ColonyActions";
import type { PaginatedResponse } from "./PaginatedResponse";

export interface ColonyDetails {
    id: number,
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