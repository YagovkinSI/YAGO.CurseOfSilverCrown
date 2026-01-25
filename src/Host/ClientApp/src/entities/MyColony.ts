import type { MyDataResponse } from "./MyDataResponse";
import { apiRequester } from "../shared/ApiRequester";
import type { ColonyParameterResponseType } from "./ColonyDetails";

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
    colonyParameters: Readonly<Record<ColonyParameterResponseType, number>>
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<MyDataResponse<MyColony>, void>({
            query: () => 'me/colony/get',
            providesTags: ['MyColony'],
        }),
    }),
});


export const {
    useGetMyColonyQuery,
} = extendedApiSlice;