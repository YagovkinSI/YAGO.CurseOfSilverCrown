import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { EventResultSlide } from "../events/EventResultSlide";

export interface MyTurn {
    id: string,
    colonyId: string,
    startAtUtc: string;
    runAtUtc: string | undefined,
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyTurn: builder.query<ApiResponse<MyTurn>, void>({
            query: () => '/me/turn/getMyTurn',
            providesTags: ['MyTurn'],
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
    useGetMyTurnQuery,
    useRunTurnMutation,
} = extendedApiSlice;