import { apiRequester} from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { Episode, Slide } from "./Episode";

export const QuestType = {
    Unknown: 0 as const,
    Default: 1 as const,
    Comleted: 2 as const,
    Required: 3 as const
} as const;

export type QuestType = typeof QuestType[keyof typeof QuestType];

export interface MyQuest {
    id: string,
    title: string,
    progress: string,
    completed: boolean,
    type: QuestType,
    slide: Slide
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyQuest: builder.query<ApiResponse<MyQuest>, string>({
            query: (id) => `me/colony/getColonyQuest?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
    
        completeQuest: builder.mutation<Episode, { id: string, dilemmaResolving: string }>({
            query: (body) => ({
                url: 'me/colony/completeQuest',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyCycle', 'MyColony'],
        })
    }),
});


export const {
    useGetColonyQuestQuery,
    useCompleteQuestMutation
} = extendedApiSlice;