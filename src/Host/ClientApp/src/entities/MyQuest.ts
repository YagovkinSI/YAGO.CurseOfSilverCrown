import { apiRequester} from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { Episode } from "./Episode";
import type { EventResultSlide } from "./EventResultSlide";

export const QuestType = {
    Unknown: 0 as const,
    Default: 1 as const,
    Ready: 2 as const,
    Immediately: 3 as const,
    News: 4 as const,
    Autostart: 5 as const,
} as const;

export type QuestType = typeof QuestType[keyof typeof QuestType];

export interface MyQuest {
    id: string,
    title: string,
    progress: string,
    type: QuestType,
    episode: Episode,
    isRead: boolean,
    createdAt: string,
    turnsLeft: number
}

export const GetColorForQuestType = (questTypes: QuestType[]): string =>
{
    if (questTypes.some(x => x == QuestType.Immediately))
        return 'red';
    if (questTypes.some(x => x == QuestType.Ready))
        return '#81C784';
    if (questTypes.some(x => x == QuestType.Default))
        return '#008cff';
    return '#000000';
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyQuest: builder.query<ApiResponse<MyQuest>, string>({
            query: (id) => `me/colony/getColonyQuest?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
    
        completeQuest: builder.mutation<ApiResponse<EventResultSlide | undefined>, { id: string, dilemmaResolving: string }>({
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