import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export const apiRequester = createApi({
    reducerPath: 'apiRequester',
    baseQuery: fetchBaseQuery({ baseUrl: "http://localhost/api" }),
    tagTypes: ['MapData'],
    endpoints: () => ({})
});