import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { ColonyParameter } from "./ColonyActions";

export interface MyColonyState {
    data: ApiResponse<MyColony>,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface MyColony {
    id: number,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<MyColony>, void>({
            query: () => 'me/colony/get',
            providesTags: ['MyColony'],
        }),
    }),
});


export const {
    useGetMyColonyQuery,
} = extendedApiSlice;