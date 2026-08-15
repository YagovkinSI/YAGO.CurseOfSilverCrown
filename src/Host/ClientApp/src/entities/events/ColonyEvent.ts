import { apiRequester} from "../../shared/api/ApiRequester";
import type { ApiResponse } from "../../shared/api/ApiResponse";
import type { Episode } from "./Episode";
import type { EventResultSlide } from "./EventResultSlide";

export type EventType = 'Default' | 'Autostart' | 'Urgent' | 'Quest';

export interface ColonyEvent {
    id: number,
    title: string,
    type: EventType,
    episode: Episode,
    isRead: boolean,
    createdAtUtc: string,
    turnsLeft: number
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyQuest: builder.query<ApiResponse<ColonyEvent>, number>({
            query: (id) => `/colonies/getColonyQuest?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
    
        completeQuest: builder.mutation<ApiResponse<EventResultSlide | undefined>, { colonyEventId: number, dilemmaResolving: string }>({
            query: (body) => ({
                url: '/colonies/completeQuest',
                method: 'POST',
                body: body,
            }),
            invalidatesTags: ['MyColony', 'MyBuildings'],
        }),
    
        setRead: builder.mutation<void, { colonyEventId: number }>({
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