import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { EventResultSlide } from "./gameActions.types";

export type GameActionType = 'event' | 'reform' | 'hireAdvisor' | 'endTurn';

export interface UseActionRequest {
    type: GameActionType;
    code?: string;
    value?: string;
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        useAction: builder.mutation<ApiResponse<EventResultSlide | undefined>, UseActionRequest>({
            query: (body) => ({
                url: '/gameActions/useAction',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings'],
        }),
    }),
});

export const {
    useUseActionMutation,
} = extendedApiSlice;
