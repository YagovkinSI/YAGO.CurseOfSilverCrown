import axios, { AxiosError } from "axios";

export interface RequestParams {
    path: string,
    type: IRequestType,
    data: any
}

export enum IRequestType {
    get, post
}

export interface IResponse<T> {
    data: T | undefined,
    error: string | undefined,
    success: boolean
}


const request = async (requestParams: RequestParams)
    : Promise<IResponse<any>> => {
    try {
        console.log(`request ${requestParams.path}`);
        let response;
        switch (requestParams.type) {
            case IRequestType.get:
                response = await axios.get(requestParams.path, requestParams.data);
                break;
            case IRequestType.post:
                response = await axios.post(requestParams.path, requestParams.data);
                break;
        }
        console.log(`response ${requestParams.path}`, response);
        if (!response.data)
            response.data = undefined;
        return {
            data: response.data,
            error: undefined,
            success: true
        } as IResponse<any>
    } catch (error) {
        console.log(`error ${requestParams.path}`, error);
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