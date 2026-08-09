import { apiRequester} from "../../shared/api/ApiRequester";
import type { ColonyParameter } from "../colonies/ColonyParameter";
import type { SlideButton } from "../events/Episode";

export interface ReformDetails {
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
        getReform: builder.query<ReformDetails, number>({
            query: (id) => `reforms/getReform?id=${id}`,
            keepUnusedDataFor: 0,
            providesTags: ['ReformDetails']
        }),
    }),
});


export const {
    useGetReformQuery,
} = extendedApiSlice;