import { apiRequester } from "../shared/ApiRequester"

export interface CurrentCityState {
    data: CurrentCity,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface CurrentCity {
    id: number,
    name: string,
    iserId: number,
    gold: number,
    population: number,
    military: number,
    fortifications: number,
    control: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getCurrentCity: builder.query<CurrentCity, void>({
            query: () => 'city/getCurrentCity',
            providesTags: ['CurrentCity'],
        }),
    }),
});


export const {
    useGetCurrentCityQuery
} = extendedApiSlice;