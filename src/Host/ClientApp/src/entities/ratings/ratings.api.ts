import { apiRequester } from '../../shared/api/ApiRequester';
import type { GameParameterValueStatus } from '../colonies/colony.types';
import type { RatingsResponse, RatingCode } from './ratings.types';

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getRatings: builder.query<RatingsResponse, { code: RatingCode; nonce: number }>({
            query: ({ code }) => `/ratings/getRatings?code=${code}`,
            keepUnusedDataFor: 0,
            transformResponse: (response: RatingsResponse): RatingsResponse =>
                response.map((field) => ({
                    ...field,
                    status: field.status.toLowerCase() as GameParameterValueStatus,
                })),
        }),
    }),
});

export const { useGetRatingsQuery } = extendedApiSlice;
