import type { MyDataResponse } from "./MyDataResponse";
import { apiRequester } from "../shared/ApiRequester";

export interface MyCycle {
    id: number,
    colonyId: number,
    completedUtc: string | undefined;
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyCycle: builder.query<MyDataResponse<MyCycle>, void>({
            query: () => 'me/cycle/get',
            providesTags: ['MyCycle'],
        }),
    }),
});

export const {
    useGetMyCycleQuery,
} = extendedApiSlice;