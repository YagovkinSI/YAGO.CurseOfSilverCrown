import { apiRequester } from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { ColonyParameter } from "./ColonyParameter";
import type { ColonyEvent } from "../events/ColonyEvent";
import type { EventResultSlide } from "../events/EventResultSlide";

export interface MyColony {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[],
    quests: ColonyEvent[],
    newColonyAvailable: boolean,
    solars: number,
    zonesAvailable: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<MyColony>, void>({
            query: () => '/me/colony/getMyColony',
            providesTags: ['MyColony'],
        }),

        issueDecree: builder.mutation<ApiResponse<EventResultSlide | undefined>, { decreeId: number }>({
            query: (body) => ({
                url: '/me/colony/issueDecree',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'DecreeDetails'],
        }),

        deactivateColony: builder.mutation<void, void>({
            query: (body) => ({
                url: '/me/colony/deactivateColony',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings', 'DecreeDetails'],
        })
    }),
});

export const {
    useGetMyColonyQuery,
    useIssueDecreeMutation,
    useDeactivateColonyMutation
} = extendedApiSlice;