import type { BaseQueryFn, EndpointBuilder, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from "@reduxjs/toolkit/query";
import type { MyDataResponse } from "./MyDataResponse";
import type { ApiMeta } from "./ApiMeta";
import { apiRequester, type TagType } from "../shared/ApiRequester";

export interface MyColonyState {
    data: MyDataResponse<MyColony>,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface MyColony {
    id: number,
    iserId: number,
    name: string,
    solars: number,
    solarsIncome: number,
    reputation: number,
    population: number,
    zonesOccupied: number,
    zonesTotal: number,
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Pragmatist: 2 as const,
    Dictator: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];

const createMyDataMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<MyDataResponse<MyColony>, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getMyColony', undefined, data)
            );
        },
        invalidatesTags: ['MyColony']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<MyDataResponse<MyColony>, void>({
            query: () => 'me/colony/get',
            providesTags: ['MyColony'],
        }),

        createColony: createMyDataMutation<{
            name: string;
            presetType: ColonyPresetType;
        }>('/me/colony/createColony', builder),
    }),
});


export const {
    useGetMyColonyQuery,
    useCreateColonyMutation,
} = extendedApiSlice;