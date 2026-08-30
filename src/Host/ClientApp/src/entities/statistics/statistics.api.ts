import { apiRequester } from '../../shared/api/ApiRequester';
import type { GameParameterValueStatus } from '../colonies/colony.types';
import type { Statistics, StatisticCode, StatisticField } from './statistics.types';

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getStatistics: builder.query<Statistics, StatisticCode>({
            query: (code) => `/statistics/getStatistics?code=${code}`,
            providesTags: ['MyColony'],
            transformResponse: (response: Statistics): Statistics => ({
                ...response,
                fields: response.fields.map((field) => ({
                    ...field,
                    status: field.status.toLowerCase() as GameParameterValueStatus,
                })),
            }),
        }),

        getColonyHeaderParameters: builder.query<StatisticField[], void>({
            query: () => `/statistics/getHeaderParameters`,
            providesTags: ['MyColony'],
            transformResponse: (response: StatisticField[]): StatisticField[] =>
                response.map((field) => ({
                    ...field,
                    status: field.status.toLowerCase() as GameParameterValueStatus,
                })),
        }),
    }),
});

export const { useGetStatisticsQuery, useGetColonyHeaderParametersQuery } = extendedApiSlice;
