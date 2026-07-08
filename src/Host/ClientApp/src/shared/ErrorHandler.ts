import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";

function isFetchBaseQueryError(error: unknown): error is FetchBaseQueryError {
    return typeof error == 'object' && error != null && 'status' in error;
}

export default function isErrorWithStatus(
    error: unknown,
    status: number
): error is FetchBaseQueryError {
    return isFetchBaseQueryError(error) && error.status == status;
}