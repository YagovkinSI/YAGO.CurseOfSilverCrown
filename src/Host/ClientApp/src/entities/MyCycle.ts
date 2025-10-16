import type { BaseQueryFn, EndpointBuilder, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from "@reduxjs/toolkit/query";
import type { MyDataResponse } from "./MyDataResponse";
import type { ApiMeta } from "./ApiMeta";
import { apiRequester, type TagType } from "../shared/ApiRequester";

export interface MyColonyState {
    data: MyDataResponse<MyCycle>,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface MyCycle {
    id: number,
    userId: number,
}

const createMyDataMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<MyDataResponse<MyCycle>, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getMyCycle', undefined, data)
            );
        },
        invalidatesTags: ['MyCycle']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyCycle: builder.query<MyDataResponse<MyCycle>, void>({
            query: () => 'me/cycle/get',
            providesTags: ['MyCycle'],
        }),

        runCyrcle: createMyDataMutation('/me/cycle/runCycle', builder),
    }),
});


export const {
    useGetMyCycleQuery,
    useRunCyrcleMutation
} = extendedApiSlice;