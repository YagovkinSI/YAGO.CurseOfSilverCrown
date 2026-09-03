import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { baseUrl } from './baseUrl';

const tagTypes = ['UserPrivate', 'MyColony', 'DecreeDetails', 'MyBuildings', 'ReformDetails', 'ReformList', 'WikiSummaries', 'WikiArticle'] as const;
export type TagType = typeof tagTypes[number];

export const apiRequester = createApi({
    reducerPath: 'apiRequester',
    baseQuery: fetchBaseQuery({ baseUrl: baseUrl }),
    tagTypes: tagTypes,
    endpoints: () => ({})
});