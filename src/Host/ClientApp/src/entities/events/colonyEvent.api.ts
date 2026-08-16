import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { ColonyEvent, EventResultSlide } from "./colonyEvent.types";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyEvent: builder.query<ApiResponse<ColonyEvent>, number>({
            query: (id) => `/events/getColonyEvent?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),

        completeEvent: builder.mutation<ApiResponse<EventResultSlide | undefined>, { colonyEventId: number; dilemmaResolving: string; }>({
            query: (body) => ({
                url: '/events/completeEvent',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings'],
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
    useCompleteEventMutation, 
    useSetReadMutation,
} = extendedApiSlice;
