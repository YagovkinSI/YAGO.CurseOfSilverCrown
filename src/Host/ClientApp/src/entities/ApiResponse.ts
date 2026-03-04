import type { Slide } from "./ColonyActions";
import type { UpdatedEntities } from "./UpdatedEntities";

export interface ApiPagination {
    total: number,
    page: number,
    limit: number
}

export interface ApiMeta {
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
    meta: ApiMeta | undefined,
    updatedEntities: UpdatedEntities | undefined,
    notification: Slide | undefined
}