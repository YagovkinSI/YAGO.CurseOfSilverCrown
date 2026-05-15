import { apiRequester} from "../shared/ApiRequester";
import type { ApiResponse } from "./ApiResponse";
import type { ColonyParameter } from "./ColonyParameter";

export interface ColonyQuest {
    id: number,
    name: string,
    image: string,
    text: string[],
    parameters: ColonyParameter[],
    description: string[]
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