import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { Episode } from "./Episode";

export interface MyCycle {
    id: number,
    colonyId: number,
    stepNumber: number,
    startAtUtc: string;
    runAtUtc: string | undefined
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyCycle: builder.query<ApiResponse<MyCycle>, void>({
            query: () => '/me/cycle/getMyCycle',
            providesTags: ['MyCycle'],
        }),
                
        runCycle: builder.mutation<ApiResponse<Episode>, void>({
            query: (body) => ({
                url: '/me/cycle/runCycle',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony'],
        }),

        setChoice: builder.mutation<void, { choiceId: string }>({
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