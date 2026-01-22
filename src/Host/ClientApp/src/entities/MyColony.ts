import type { MyDataResponse } from "./MyDataResponse";
import { apiRequester } from "../shared/ApiRequester";
import type { ColonyPresetType } from "./ColonyActions";

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
    gavernorType: number,
    population: number,
    zonesOccupied: number,
    zonesTotal: number,
    codeOfLaws: ColonyPresetType,
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