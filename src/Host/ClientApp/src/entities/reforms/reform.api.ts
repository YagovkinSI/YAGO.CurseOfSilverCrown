import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { EventResultSlide } from "../events/colonyEvent.types";
import type { ReformDetails, ReformSummary } from "./reform.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getReforms: builder.query<ReformSummary[], void>({
            query: () => `/reforms/getReforms`,
            keepUnusedDataFor: 0,
            providesTags: ['ReformList']
        }),

        getReform: builder.query<ReformDetails, string>({
            query: (code) => `/reforms/getReform?code=${code}`,
            keepUnusedDataFor: 0,
            providesTags: ['ReformDetails']
        }),

        setReform: builder.mutation<ApiResponse<EventResultSlide | undefined>, { reformCode: string, reformValue: string, }>({
            query: (body) => ({
                url: '/reforms/setReform',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'ReformDetails', 'ReformList'],
        }),
    }),

});

export const {
    useGetReformsQuery,
    useGetReformQuery,
    useSetReformMutation,
} = extendedApiSlice;