import { apiRequester} from "../shared/ApiRequester";
import type { ColonyParameter } from "./ColonyParameter";

export interface DecreeDetails {
    id: number,
    name: string,
    image: string,
    text: string[],
    parameters: ColonyParameter[],
    description: string[]
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getDecree: builder.query<DecreeDetails, number>({
            query: (id) => `decrees/getDecree?id=${id}`,
            providesTags: (_, __, id) => [
                { type: 'DecreeDetails', id },
                { type: 'DecreeDetails', id: 'LIST' }
            ],
        }),
    }),
});


export const {
    useGetDecreeQuery,
} = extendedApiSlice;