import { apiRequester} from "../shared/ApiRequester";

export interface UnitDetails {
    id: number,
    name: string,
    cost: number,
    zonesOccupied: number,
    solarsIncome: number,
    gavernorType: number,
    population: number,
    text: string[],
    description: string[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getUnit: builder.query<UnitDetails, number>({
            query: (id) => `units/get?id=${id}`,
            providesTags: (_, __, id) => [
                { type: 'UnitDetails', id },
                { type: 'UnitDetails', id: 'LIST' }
            ],
        }),
    }),
});


export const {
    useGetUnitQuery,
} = extendedApiSlice;