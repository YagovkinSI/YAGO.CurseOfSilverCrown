import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

const baseUrl : string = 'https://localhost:44308/api';
//const baseUrl : string = 'http://localhost/api';
//const baseUrl : string = 'http://89.111.153.37/api'

const tagTypes = ['AuthorizationData', 'Playthrough', 'Story', 'StoryList'] as const;
export type TagType = typeof tagTypes[number];

export const apiRequester = createApi({
    reducerPath: 'apiRequester',
    baseQuery: fetchBaseQuery({ baseUrl: baseUrl }),
    tagTypes: tagTypes,
    endpoints: () => ({})
});