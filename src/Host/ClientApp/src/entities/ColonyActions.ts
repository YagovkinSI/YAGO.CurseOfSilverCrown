import type { BaseQueryFn, EndpointBuilder, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from "@reduxjs/toolkit/query";
import { apiRequester, type TagType } from "../shared/ApiRequester";
import type { MyCycle } from "./MyCycle";
import type { ApiMeta } from "./ApiMeta";
import type { ThunkDispatch, UnknownAction } from "@reduxjs/toolkit";
import type { MyDataResponse } from "./MyDataResponse";
import type { MyColony } from "./MyColony";

export interface UpdatedColonyEntities {
    myCycle: MyCycle | undefined,
    myColony: MyColony | undefined
}

export interface Notification {
    message: string //fake
}

export interface ColonyActionResponse {
    notification: Notification | undefined,
    updatedEntities: UpdatedColonyEntities
}

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
    updatedEntities: UpdatedColonyEntities,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    dispatch: ThunkDispatch<any, any, UnknownAction>
) => {

    if (updatedEntities.myCycle) {
        const value: MyDataResponse<MyCycle> = { isAuthorized: true, data: updatedEntities.myCycle }
        updateEntityCache('getMyCycle', value, dispatch);

    }

    if (updatedEntities.myColony) {
        const value: MyDataResponse<MyColony> = { isAuthorized: true, data: updatedEntities.myColony }
        updateEntityCache('getMyColony', value, dispatch);
    }
};

const createMyDataMutation = <BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">,
    invalidatesTags: ("MyUser" | "MyColony" | "MyCycle")[] = []
) => {
    return builder.mutation<ColonyActionResponse, BodyType>({
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
        runCycle:
            createMyDataMutation(
                'colony-actions/runCycle',
                builder),
    }),
});

export const {
    useRunCycleMutation
} = extendedApiSlice;