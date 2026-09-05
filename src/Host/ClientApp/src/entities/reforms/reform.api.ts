import { apiRequester } from "../../shared/api/ApiRequester";
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
    }),

});

export const {
    useGetReformsQuery,
    useGetReformQuery,
} = extendedApiSlice;