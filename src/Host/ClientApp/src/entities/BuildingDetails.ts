import { apiRequester} from "../shared/ApiRequester";

export interface BuildingDetails {
    id: number,
    name: string,
    cost: number,
    zonesOccupied: number,
    solarsIncome: number,
    stability: number,
    population: number,
    description: string[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getBuilding: builder.query<BuildingDetails, number>({
            query: (id) => `buildings/get?id=${id}`,
            providesTags: (_, __, id) => [
                { type: 'BuildingDetails', id },
                { type: 'BuildingDetails', id: 'LIST' }
            ],
        }),
    }),
});


export const {
    useGetBuildingQuery,
} = extendedApiSlice;