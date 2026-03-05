import type { ThunkDispatch, UnknownAction } from "@reduxjs/toolkit";
import type { Slide } from "./ColonyActions";
import type { UpdatedEntities } from "./UpdatedEntities";
import type { MyCycle } from "./MyCycle";
import type { MyDataResponse } from "./MyDataResponse";
import type { MyColony } from "./MyColony";
import { apiRequester, type TagType } from "../shared/ApiRequester";
import type { BaseQueryFn, EndpointBuilder, FetchArgs, FetchBaseQueryError, FetchBaseQueryMeta } from "@reduxjs/toolkit/query";

export type ApiMeta = {
    cacheControl?: string;
    metrics?: {
        duration: number;
    };
};

export interface ApiPagination {
    total: number,
    page: number,
    limit: number
}

export interface ApiResponseMeta {
    pagination: ApiPagination | undefined
}

export interface ApiError {
    code: string,
    mwessage: string,
    details: string | undefined
}

export interface ApiResponse<T> {
    success: boolean,
    data: T | undefined,
    error: ApiError | undefined,
    meta: ApiResponseMeta | undefined,
    updatedEntities: UpdatedEntities | undefined,
    notification: Slide | undefined
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
    updatedEntities: UpdatedEntities | undefined,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    dispatch: ThunkDispatch<any, any, UnknownAction>
) => {

    if (!updatedEntities)
        return;

    if (updatedEntities.myCycle) {
        const value: MyDataResponse<MyCycle> = { isAuthorized: true, data: updatedEntities.myCycle }
        updateEntityCache('getMyCycle', value, dispatch);

    }

    if (updatedEntities.myColony) {
        const value: MyDataResponse<MyColony> = { isAuthorized: true, data: updatedEntities.myColony }
        updateEntityCache('getMyColony', value, dispatch);
    }
};

export const createMyDataMutation = <T, BodyType extends Record<string, unknown>>(
    url: string,
    builder: EndpointBuilder<BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError, ApiMeta, FetchBaseQueryMeta>, TagType, "apiRequester">,
    invalidatesTags: ("MyUser" | "MyColony" | "MyCycle")[] = []
) => {
    return builder.mutation<ApiResponse<T>, BodyType>({
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