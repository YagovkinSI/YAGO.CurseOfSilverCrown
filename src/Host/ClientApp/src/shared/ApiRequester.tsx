import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

const baseUrl : string = 'https://localhost:44308/api';
//const baseUrl : string = 'http://localhost/api';
//const baseUrl : string = 'http://89.111.153.37/api'

export const apiRequester = createApi({
    reducerPath: 'apiRequester',
    baseQuery: fetchBaseQuery({ baseUrl: baseUrl }),
    tagTypes: ['CurrentUser', 'CurrentStory'],
    endpoints: () => ({})
});