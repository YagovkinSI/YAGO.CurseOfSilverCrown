import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";

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
            invalidatesTags: ['MyCycle', 'MyColony', 'MyBuildings'],
        })
    }),
});

export const {
    useGetMyCycleQuery,
    useRunCycleMutation,
} = extendedApiSlice;