import { createAsyncThunk } from "@reduxjs/toolkit";
import axios, { AxiosError } from "axios";

export interface RequestParams {
    path: string,
    type: RequestType,
    data: any
}

export enum RequestType {
    Get, Post
}

interface Response<T> {
    data?: T,
    error?: string,
    success: boolean
}

const createThunk = (typePrefix: string) => {
    return createAsyncThunk(
        typePrefix,
        async (requestParams: RequestParams, thunkAPI) => {
            const response = await request(requestParams);
            if (response.success) {
                return thunkAPI.fulfillWithValue(response.data);
            } else {
                const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
                return thunkAPI.rejectWithValue(error);
            }
        }
    )
}

const request = async (requestParams: RequestParams)
    : Promise<Response<any>> => {
    try {
        console.log(`request ${requestParams.path}`);
        let response;
        switch (requestParams.type) {
            case RequestType.Get:
                response = await axios.get(requestParams.path, requestParams.data);
                break;
            case RequestType.Post:
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
        } as Response<any>
    } catch (error) {
        console.log(`error ${requestParams.path}`, error);
        const errorMessage = getErrorMessage(error as AxiosError);
        return {
            data: undefined,
            error: errorMessage,
            success: false
        } as Response<any>
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

export const requester = { createThunk };