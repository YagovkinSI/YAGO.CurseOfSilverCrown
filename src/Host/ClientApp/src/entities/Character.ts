import type { BaseQueryFn, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from '@reduxjs/toolkit/query';
import type { EndpointBuilder } from '@reduxjs/toolkit/query';
import { apiRequester, type TagType } from "../shared/ApiRequester"
import type { ApiMeta } from './ApiMeta';

export interface Character {
    id: number,
    userId: number,
    name: string,
    race: 'Unknown' | 'Isian' | 'Nahumi' | 'Daji' | 'Khashin' | 'Elnir' | 'Khazadin',
    gender: 'Unknown' | 'Male' | 'Female',
    background: 'Unknown' | 'Mercenary' | 'Emissary' | 'Spy',
    force: number,
    diplomacy: number,
    cunning: number,
    inventory: string,
    image: string
}

export const createCharacterMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">
) => {
    return builder.mutation<Character, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            const { data } = await queryFulfilled;
            dispatch(
                extendedApiSlice.util.upsertQueryData('getCurrentCharacter', undefined, data)
            );
        },
        invalidatesTags:['StoryList', 'Story']
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        getCurrentCharacter: builder.query<Character, void>({
            query: () => 'character/get',
            providesTags: ['CurrentCharacter'],
        }),

        createCharacter: createCharacterMutation<{
            character: Character
        }>('/character/create', builder)
    }),
});

export const {
    useGetCurrentCharacterQuery,
    useCreateCharacterMutation,
} = extendedApiSlice;