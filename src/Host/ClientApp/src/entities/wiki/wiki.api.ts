import { apiRequester } from "../../shared/api/ApiRequester";
import type { WikiArticle, WikiSummary } from "./wiki.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getWikiSummaries: builder.query<WikiSummary[], void>({
            query: () => `/wiki/getWikiSummaries`,
            keepUnusedDataFor: 0,
            providesTags: ['WikiSummaries']
        }),

        getWikiArticle: builder.query<WikiArticle, string>({
            query: (code) => `/wiki/getWikiArticle?code=${code}`,
            keepUnusedDataFor: 0,
            providesTags: ['WikiArticle'],
            onQueryStarted: async (_arg, { dispatch, queryFulfilled }) => {
                try {
                    await queryFulfilled;
                    dispatch(apiRequester.util.invalidateTags(['MyColony']));
                } catch {
                    return;
                }
            },
        }),
    }),
});

export const {
    useGetWikiSummariesQuery,
    useGetWikiArticleQuery,
} = extendedApiSlice;
