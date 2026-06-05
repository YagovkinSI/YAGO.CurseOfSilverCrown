import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";

export interface MyCycle {
    id: string,
    colonyId: string,
    startAtUtc: string;
    runAtUtc: string | undefined,
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyCycle: builder.query<ApiResponse<MyCycle>, void>({
            query: () => '/me/cycle/getMyCycle',
            providesTags: ['MyCycle'],
        }),
                
        runCycle: builder.mutation<MyCycle, void>({
            query: (body) => ({
                url: '/me/cycle/runCycle',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony'],
        }),

        setChoice: builder.mutation<void, { eventId: string, dilemmaResolving: string }>({
            query: (body) => ({
                url: '/me/cycle/setChoice',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony'],
        })
    }),
});

export const {
    useGetMyCycleQuery,
    useRunCycleMutation,
    useSetChoiceMutation
} = extendedApiSlice;