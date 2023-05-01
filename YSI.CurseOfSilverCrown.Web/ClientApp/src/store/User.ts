import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch, useAppSelector } from ".";
import { RequestType, requester, RequestParams } from "../requester";

export interface UserState {
    user: IUserPrivate | undefined,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export interface IUserPrivate {
    id: string
    userName: string
}

export const defaultUserState: UserState = {
    user: undefined,
    isLoading: false,
    isChecked: false,
    error: ''
}

const request = requester.createThunk('user');

export const userSlice = createSlice({
    name: 'user',
    initialState: defaultUserState,
    reducers: {},
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<IUserPrivate>) => {
            state.user = action.payload
            state.isChecked = true,
                state.isLoading = false,
                state.error = ''
        },
        [request.pending.type]: (state) => {
            state.isChecked = true,
                state.isLoading = true,
                state.error = ''
        },
        [request.rejected.type]: (state, action: PayloadAction<string>) => {
            state.isChecked = true,
                state.isLoading = false,
                state.error = action.payload
        }
    }
});

const getCurrentUser = async (dispatch: AppDispatch) => {
    const state = useAppSelector(state => state.userReducer);
    if (state.isChecked)
        return;

    const requestParams: RequestParams = {
        path: 'apiUser/getCurrentUser',
        type: RequestType.Get,
        data: {}
    }
    dispatch(request(requestParams));
}

const register = async (dispatch: AppDispatch,
    userName: string, password: string, passwordConfirm: string) => {

    const requestParams: RequestParams = {
        path: 'apiUser/register',
        type: RequestType.Post,
        data: { userName, password, passwordConfirm }
    }
    await dispatch(request(requestParams));
}

const login = async (dispatch: AppDispatch, userName: string, password: string) => {
    const requestParams: RequestParams = {
        path: 'apiUser/login',
        type: RequestType.Post,
        data: { userName, password }
    }
    await dispatch(request(requestParams));
}

const logout = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'apiUser/logout',
        type: RequestType.Post,
        data: {}
    }
    await dispatch(request(requestParams));
}

export const userActionCreators = { register, login, logout, getCurrentUser };


