import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { ColonyParameter, ColonyPresetType } from "./ColonyActions";

export interface MyColony {
    id: number,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<MyColony>, void>({
            query: () => '/me/colony/getMyColony',
            providesTags: ['MyColony'],
        }),
        
        createColony: builder.mutation<void, { name: string; presetType: ColonyPresetType; }>({
            query: (body) => ({
                url: '/me/colony/createColony',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony'],
        }),

        issueDecree: builder.mutation<void, { decreeId: number }>({
            query: (body) => ({
                url: '/me/colony/issueDecree',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony'],
        }),

        deactivateColony: builder.mutation<void, void>({
            query: (body) => ({
                url: '/me/colony/deactivateColony',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony'],
        })
    }),
});

export const {
    useGetMyColonyQuery,
    useCreateColonyMutation,
    useIssueDecreeMutation,
    useDeactivateColonyMutation
} = extendedApiSlice;