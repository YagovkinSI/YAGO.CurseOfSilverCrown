import { apiRequester } from "../shared/ApiRequester";
import type { PaginatedResponse } from "./PaginatedResponse";

export const ColonyParameterResponseType = {
    Unknown: "Unknown" as const,
    Solars: "Solars" as const,
    SolarsIncome: "SolarsIncome" as const,
    GavernorType: "GavernorType" as const,
    Population: "Population" as const,
    ZonesOccupied: "ZonesOccupied" as const,
    ZonesTotal: "ZonesTotal" as const,
    CodeOfLaws: "CodeOfLaws" as const,
    Ship: "Ship" as const,
} as const;

export type ColonyParameterResponseType = typeof ColonyParameterResponseType[keyof typeof ColonyParameterResponseType]

export interface ColonyDetails {
    id: number,
    iserId: number,
    name: string,
    colonyParameters: Readonly<Record<ColonyParameterResponseType, number>>
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