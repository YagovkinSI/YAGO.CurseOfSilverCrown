import { apiRequester} from "../../shared/api/ApiRequester";
import type { ColonyParameter } from "../colonies/ColonyParameter";
import type { SlideButton } from "../events/Episode";

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