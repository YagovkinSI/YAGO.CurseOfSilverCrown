export interface MyDataResponse<T> {
    isAuthorized: boolean;
    data: T | undefined;
    readyDateTimeUtc?: string | undefined;
}