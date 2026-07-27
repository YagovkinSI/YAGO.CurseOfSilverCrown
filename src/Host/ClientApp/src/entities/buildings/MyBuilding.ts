import { apiRequester } from "../../shared/api/ApiRequester";

export interface MyBuildingBase {
    isPrivate: boolean;
    buildingCount: number;
    buildAvailable: boolean;
    unavailabilityReason: string | null;
    cost: number;
}

export interface MyBuilding {
    type: BuildType;
    name: string;
    imageName: string;
    description: string[];
    private: MyBuildingBase;
    state: MyBuildingBase;
}

export type BuildType = "Administrative" | "Mining" | "Service" | "Production";

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getBuildings: builder.query<MyBuilding[], void>({
            query: () => '/buildings/getBuildings',
            providesTags: ['MyBuildings'],
        }),
                
        build: builder.mutation<void, { buildType : BuildType, isPrivate :boolean }>({
            query: (body) => ({
                url: '/buildings/build',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony', 'MyBuildings'],
        })
    }),
});

export const {
    useGetBuildingsQuery,
    useBuildMutation,
} = extendedApiSlice;