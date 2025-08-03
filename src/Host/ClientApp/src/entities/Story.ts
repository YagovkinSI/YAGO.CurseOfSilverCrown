import { apiRequester } from "../shared/ApiRequester";
import type { Slide } from "./CurrentStoryNode";
import type YagoEntity from "./YagoEntity";

export interface StoryState {
    data: Story,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface Story {
    user: YagoEntity,
    gameSession: YagoEntity,
    chapter: number,
    title: string,
    slides: Slide[]
}

const defaultStory: Story = {
    user: { id: 0, name: "Ошибка получения данных", type: 'User' },
    gameSession: { id: 0, name: "Ошибка получения данных", type: 'GameSession' },
    chapter: 0,
    title: "Ошибка получения данных",
    slides: []
}

export const defaultStoryState: StoryState = {
    data: defaultStory,
    isLoading: false,
    isChecked: false,
    error: ''
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getStory: builder.query<Story, number>({
            query: (id) => `story/getStory?id=${id}`,
            providesTags: (_result, _error, id) => [{ type: 'Story', id }],
        }),
    }),
});


export const {
    useGetStoryQuery
} = extendedApiSlice;