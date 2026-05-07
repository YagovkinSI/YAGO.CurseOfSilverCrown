import { apiRequester } from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { ColonyParameter } from "./ColonyParameter";

export interface MyColony {
    id: string,
    iserId: number,
    name: string,
    colonyParameters: ColonyParameter[],
    quests: MyQuest[],
    autoRunCycle: boolean,
    newColonyAvailable: boolean,
    solars: number,
    zonesAvailable: number
}

export const QuestType = {
    Unknown: 0 as const,
    Default: 1 as const,
    Comleted: 2 as const,
    Required: 3 as const
} as const;

export type QuestType = typeof QuestType[keyof typeof QuestType];

export interface MyQuest {
    id: string,
    name: string,
    progress: string,
    type: QuestType
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getMyColony: builder.query<ApiResponse<MyColony>, void>({
            query: () => '/me/colony/getMyColony',
            providesTags: ['MyColony'],
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
    useIssueDecreeMutation,
    useDeactivateColonyMutation
} = extendedApiSlice;