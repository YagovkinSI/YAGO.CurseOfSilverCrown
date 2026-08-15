import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { PaginatedResponse } from "../../shared/api/PaginatedResponse";
import type { EventResultSlide } from "../events/EventResultSlide";
import type { ColonyDetails, ColonyPrivate } from "./colony.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<ColonyPrivate>, void>({
            query: () => '/me/colony/getMyColony',
            providesTags: ['MyColony'],
        }),
        
        createColony: builder.mutation<ApiResponse<ColonyPrivate>, void>({
            query: (body) => ({
                url: '/colonies/createColony',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'ReformDetails'],
        }),

        issueReform: builder.mutation<ApiResponse<EventResultSlide | undefined>, { reformId: number }>({
            query: (body) => ({
                url: '/me/colony/issueReform',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'ReformDetails'],
        }),

        getColonyRaiting: builder.query<PaginatedResponse<ColonyDetails>, { page: number }>({
            query: ({ page }) => `colonies/getColonyRaiting?page=${page}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
        
        runTurn: builder.mutation<ApiResponse<EventResultSlide | undefined>, void>({
            query: (body) => ({
                url: '/me/turn/runTurn',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyTurn', 'MyColony', 'MyBuildings'],
        })
    }),
});

export const {
    useGetMyColonyQuery,
    useLazyGetMyColonyQuery,
    useCreateColonyMutation,
    useIssueReformMutation,
    useGetColonyRaitingQuery,
    useRunTurnMutation,
} = extendedApiSlice;