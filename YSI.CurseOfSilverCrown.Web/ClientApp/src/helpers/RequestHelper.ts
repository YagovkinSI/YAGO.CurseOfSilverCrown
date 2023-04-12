import axios, { AxiosError } from "axios";

export interface IResponse<T> {
    data: T | undefined,
    error: string | undefined,
    success: boolean
}

export enum IRequestType {
    get, post
}

const request = async (apiPath: string, requestType: IRequestType, data: any)
    : Promise<IResponse<any>> => {
    try {
        console.log(`request ${apiPath}`);
        let response;
        switch (requestType) {
            case IRequestType.get:
                response = await axios.get(apiPath, data);
                break;
            case IRequestType.post:
                response = await axios.post(apiPath, data);
                break;
        }
        console.log(`response ${apiPath}`, response);
        return {
            data: response.data,
            error: undefined,
            success: true
        } as IResponse<any>
    } catch (error) {
        console.log(`error ${apiPath}`, error);
        const errorMessage = getErrorMessage(error as AxiosError);
        return {
            data: undefined,
            error: errorMessage,
            success: false
        } as IResponse<any>
    }
}

const getErrorMessage = (error: AxiosError): string => {
    if (error == undefined)
        return 'Произошла неизвестная ошибка';

    if (error.response == undefined)
        return error.message;

    const dataAsString = error.response.data as string;
    return dataAsString == undefined
        ? error.message
        : dataAsString;
}

export const requestHelper = { request };