import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { ColonyEventPrivate } from "./colonyEvent.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyEvent: builder.query<ApiResponse<ColonyEventPrivate>, number>({
            query: (id) => `/events/getColonyEvent?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),

        setRead: builder.mutation<void, { colonyEventId: number; }>({
            query: (body) => ({
                url: 'events/setRead',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony'],
        })
    }),
});


export const {
    useGetColonyEventQuery,
    useSetReadMutation,
} = extendedApiSlice;
