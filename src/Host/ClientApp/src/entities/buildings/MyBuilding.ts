import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { ColonyParameter } from "../colonies/ColonyParameter";
import type { EventResultSlide } from "../events/EventResultSlide";

export interface MyBuildingBase {
    isPrivate: boolean;
    buildingCount: number;
    buildAvailable: boolean;
    unavailabilityReason?: string;
    cost: number;
    bonuses: ColonyParameter[];
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
                
        build: builder.mutation<ApiResponse<EventResultSlide | undefined>, { buildType : BuildType, isPrivate :boolean }>({
            query: (body) => ({
                url: '/buildings/build',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony', 'MyBuildings', 'ReformDetails'],
        })
    }),
});

export const {
    useGetBuildingsQuery,
    useBuildMutation,
} = extendedApiSlice;