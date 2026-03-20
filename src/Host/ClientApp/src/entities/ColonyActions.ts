import type { BaseQueryFn, EndpointBuilder, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from "@reduxjs/toolkit/query";
import { apiRequester, type TagType } from "../shared/ApiRequester";
import type { MyCycle } from "./MyCycle";
import type { ApiMeta } from "./ApiMeta";
import type { ThunkDispatch, UnknownAction } from "@reduxjs/toolkit";
import type { MyColony } from "./MyColony";
import type { ColonyParameterName } from "./ColonyParameterType";
import type { UpdatedEntities } from "./UpdatedEntities";
import type { ApiResponse } from "./ApiResponse";

export interface ColonyParameter {
    name: ColonyParameterName,
    value: number,
    isChanging: boolean
}

export interface Episode {
    id: string | undefined,
    slides: Slide[],
    choiceLabel: string | undefined,
    choice: Slide[] | undefined
}

export interface Slide {
    title: string,
    illustration: string,
    text: string[],
    parameters: ColonyParameter[]
}

export const ColonyPresetType = {
    Unknown: 0 as const,
    Humanist: 1 as const,
    Centrist: 2 as const,
    Capitalist: 3 as const
} as const;

export type ColonyPresetType = typeof ColonyPresetType[keyof typeof ColonyPresetType];

const updateEntityCache = <T>(
    endpointName: string,
    data: T,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    dispatch: ThunkDispatch<any, any, UnknownAction>
) => {
    dispatch(
        apiRequester.util.updateQueryData(
            endpointName as never,
            undefined as never,
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            (draft: any) => {
                Object.assign(draft, data);
            }
        )
    );
};

const updateCache = (
    updatedEntities: UpdatedEntities | undefined,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    dispatch: ThunkDispatch<any, any, UnknownAction>
) => {

    if (!updatedEntities)
        return;

    if (updatedEntities.myCycle) {
        const value: ApiResponse<MyCycle> = { success: true, data: updatedEntities.myCycle }
        updateEntityCache('getMyCycle', value, dispatch);

    }

    if (updatedEntities.myColony) {
        const value: ApiResponse<MyColony> = { success: true, data: updatedEntities.myColony }
        updateEntityCache('getMyColony', value, dispatch);
    }
};

const createMyDataMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">,
    invalidatesTags: ("MyUser" | "MyColony" | "MyCycle")[] = []
) => {
    return builder.mutation<ApiResponse<Episode>, BodyType>({
        query: (body) => ({
            url,
            method: 'POST',
            body,
        }),
        async onQueryStarted(_, { dispatch, queryFulfilled }) {
            try {
                const { data: response } = await queryFulfilled;
                updateCache(response.updatedEntities, dispatch);
                dispatch(apiRequester.util.invalidateTags(invalidatesTags));
            } catch (error) {
                console.error(`Command createMyDataMutation failed:`, error);
            }
        },
        invalidatesTags
    });
};

const extendedApiSlice = apiRequester.injectEndpoints({
    endpoints: (builder) => ({

        runCycle: createMyDataMutation(
            'colony-actions/runCycle', builder, ["MyColony"]),

        issueDecree: createMyDataMutation<{
            decreeId: number
        }>(
            'colony-actions/issueDecree', builder, ["MyColony"])
    }),
});

export const {
    useRunCycleMutation,
    useIssueDecreeMutation
} = extendedApiSlice;