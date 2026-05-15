import { apiRequester} from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";

export const QuestType = {
    Unknown: 0 as const,
    Default: 1 as const,
    Comleted: 2 as const,
    Required: 3 as const
} as const;

export type QuestType = typeof QuestType[keyof typeof QuestType];

export interface ColonyQuest {
    id: string,
    name: string,
    progress: string,
    type: QuestType
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyQuest: builder.query<ApiResponse<ColonyQuest>, string>({
            query: (id) => `me/colony/getColonyQuest?id=${id}`,
        }),
    }),
});


export const {
    useGetColonyQuestQuery,
} = extendedApiSlice;