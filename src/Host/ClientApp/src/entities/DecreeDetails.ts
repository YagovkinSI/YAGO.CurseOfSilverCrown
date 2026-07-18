import { apiRequester} from "../shared/ApiRequester";
import type { ColonyParameter } from "./ColonyParameter";
import type { SlideButton } from "./Episode";

export interface DecreeDetails {
    id: number,
    name: string,
    image: string,
    text: string[],
    parameters: ColonyParameter[],
    requirements: ColonyParameter[],
    description: string[],
    button: SlideButton
}

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({
        getDecree: builder.query<DecreeDetails, number>({
            query: (id) => `decrees/getDecree?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: []
        }),
    }),
});


export const {
    useGetDecreeQuery,
} = extendedApiSlice;