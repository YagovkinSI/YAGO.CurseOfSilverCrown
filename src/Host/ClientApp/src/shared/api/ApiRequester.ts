import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

//const baseUrl : string = 'https://localhost:5001/api';
//const baseUrl : string = 'http://localhost/api';
const baseUrl : string = 'http://95.163.227.105/api'

const tagTypes = ['MyUser', 'MyColony', 'MyCycle', 'DecreeDetails', 'MyBuildings'] as const;
export type TagType = typeof tagTypes[number];

export const apiRequester = createApi({
    reducerPath: 'apiRequester',
    baseQuery: fetchBaseQuery({ baseUrl: baseUrl }),
    tagTypes: tagTypes,
    endpoints: () => ({})
});