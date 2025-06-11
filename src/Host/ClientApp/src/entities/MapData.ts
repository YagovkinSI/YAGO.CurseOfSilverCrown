import { apiRequester } from "../shared/ApiRequester";

export interface MapDataState {
    data: MapElementDictionary,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

type MapElementDictionary = Record<string, MapElement>;

export interface MapElement {
    name: string,
    colorStr: string,
    info: string[]
}

export const defaultMapDataState: MapDataState = {
    data: {},
    isLoading: false,
    isChecked: false,
    error: ''
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: builder => ({

        index: builder.query<MapElementDictionary, void>({
            query: () => `/map`,
            providesTags: [{ type: 'Daily', id: 'Map' }]
        }),
    })
})

export const { useIndexQuery } = extendedApiSlice;