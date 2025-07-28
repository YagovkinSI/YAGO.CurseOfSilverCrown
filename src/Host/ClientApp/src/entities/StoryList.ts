import { apiRequester } from "../shared/ApiRequester";
import type { PaginatedResponse } from "./PaginatedResponse";
import type YagoEntity from "./YagoEntity";

export interface StoryListState {
    data: PaginatedResponse<StoryItem>,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface StoryItem {
    id: number,
    user: YagoEntity,
    gameSession: YagoEntity,
    chapter: number,
    title: string
}

const defaultStoryList: PaginatedResponse<StoryItem> =
{
    data: [],
    limit: 10,
    page: 1,
    total: 0
}

export const defaultStoryListState: StoryListState = {
    data: defaultStoryList,
    isLoading: false,
    isChecked: false,
    error: ''
}

const extendedApiSlice = apiRequester.injectEndpoints({

    endpoints: (builder) => ({
        getStoryList: builder.query<PaginatedResponse<StoryItem>, number>({
            query: (page) => `story/getStoryList?page=${page}`,
            providesTags: ['StoryList'],
        }),
    }),

});


export const {
    useGetStoryListQuery
} = extendedApiSlice;