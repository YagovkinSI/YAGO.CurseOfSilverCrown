import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { ColonyPrivate } from "./colony.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<ColonyPrivate>, void>({
            query: () => '/colonies/getMyColony',
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
    }),
});

export const {
    useGetMyColonyQuery,
    useLazyGetMyColonyQuery,
    useCreateColonyMutation,
} = extendedApiSlice;
