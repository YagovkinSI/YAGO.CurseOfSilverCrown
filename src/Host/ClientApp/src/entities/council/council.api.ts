import { apiRequester } from "../../shared/api/ApiRequester";
import type { CouncilPosition } from "./council.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getCouncilPositions: builder.query<CouncilPosition[], void>({
            query: () => `/council/getCouncilPositions`,
            keepUnusedDataFor: 0,
            providesTags: ['CouncilPositions']
        }),
    }),
});

export const {
    useGetCouncilPositionsQuery,
} = extendedApiSlice;