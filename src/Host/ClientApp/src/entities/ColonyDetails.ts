import { apiRequester } from "../shared/ApiRequester";
import type { PaginatedResponse } from "./PaginatedResponse";

export interface ColonyDetails {
    id: number,
    iserId: number,
    name: string,
    solarsIncome: number,
    reputation: number,
    population: number,
    zonesOccupied: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColony: builder.query<PaginatedResponse<ColonyDetails>, number>({
            query: (id) => `colonies/get?id=${id}`,
            providesTags: (_, __, id) => [
                { type: 'ColonyDetails', id },
                { type: 'ColonyDetails', id: 'LIST' }
            ],
        }),

        getColonyRaiting: builder.query<PaginatedResponse<ColonyDetails>, { page: number }>({
            query: ({ page }) => `colonies/getColonyRaiting?page=${page}`,
            providesTags: (result, _, { page }) => {
                const tags: Array<{ type: 'ColonyDetails', id: number | string }> = [
                    { type: 'ColonyDetails', id: `page-${page}` },
                    { type: 'ColonyDetails', id: 'LIST' }
                ];

                if (result?.data) {
                    tags.push(
                        ...result.data.map(c => ({
                            type: 'ColonyDetails' as const,
                            id: c.id
                        }))
                    );
                }

                return tags;
            },
        }),
    }),
});


export const {
    useGetColonyRaitingQuery,
} = extendedApiSlice;