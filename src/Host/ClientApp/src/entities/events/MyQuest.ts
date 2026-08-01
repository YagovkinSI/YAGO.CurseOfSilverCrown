import { apiRequester} from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { Episode } from "./Episode";
import type { EventResultSlide } from "./EventResultSlide";

export type EventType = 'Default' | 'Autostart' | 'Urgent' | 'Quest';

export interface MyQuest {
    id: string,
    title: string,
    type: EventType,
    episode: Episode,
    isRead: boolean,
    createdAtUtc: string,
    turnsLeft: number
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
            invalidatesTags: ['MyCycle', 'MyColony', 'MyBuildings'],
        }),
    
        setRead: builder.mutation<void, { eventId: string }>({
            query: (body) => ({
                url: 'events/setRead',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony'],
        })
    }),
});


export const {
    useGetColonyQuestQuery,
    useCompleteQuestMutation,
    useSetReadMutation,
} = extendedApiSlice;