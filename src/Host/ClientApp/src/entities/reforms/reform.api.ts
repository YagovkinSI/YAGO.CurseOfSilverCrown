import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { EventResultSlide } from "../events/colonyEvent.types";
import type { ReformDetails } from "./reform.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getReform: builder.query<ReformDetails, string>({
            query: (code) => `reforms/getReform?code=${code}`,
            keepUnusedDataFor: 0,
            providesTags: ['ReformDetails']
        }),

        issueReform: builder.mutation<ApiResponse<EventResultSlide | undefined>, { reformCode: string }>({
            query: (body) => ({
                url: '/colonies/issueReform',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'ReformDetails'],
        }),
    }),

});

export const {
    useGetReformQuery,
    useIssueReformMutation,
} = extendedApiSlice;