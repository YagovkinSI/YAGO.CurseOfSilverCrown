import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";

export interface MyCycle {
    id: number,
    colonyId: number,
    stepNumber: number,
    runAtUtc: string | undefined;
    state: CycleState
}

export const CycleState = {
    Unknown: 0 as const,
    Ready: 1 as const,
    InProgress: 2 as const,
    Completed: 3 as const,
} as const;

export type CycleState = typeof CycleState[keyof typeof CycleState]

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyCycle: builder.query<ApiResponse<MyCycle>, void>({
            query: () => 'me/cycle/get',
            providesTags: ['MyCycle'],
        }),
    }),
});

export const {
    useGetMyCycleQuery,
} = extendedApiSlice;