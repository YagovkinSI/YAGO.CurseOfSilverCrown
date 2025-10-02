export interface MyDataResponse<T> {
    isAuthorized: boolean;
    data: T | undefined;
}