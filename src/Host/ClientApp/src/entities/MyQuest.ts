import { apiRequester} from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { PrologueSlide } from "./Episode";

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
    completed: boolean,
    type: QuestType,
    prologueSlide: PrologueSlide
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getColonyQuest: builder.query<ApiResponse<MyQuest>, string>({
            query: (id) => `me/colony/getColonyQuest?id=${id}`,
        }),
    }),
});


export const {
    useGetColonyQuestQuery,
} = extendedApiSlice;