import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch, useAppSelector } from ".";
import IUserPrivate from "../apiModels/userPrivate";
import { IRequestType, requestHelper, RequestParams } from "../helpers/RequestHelper";

export interface UserState {
    isSignedIn: boolean,
    userName: string,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export const defaultUserState: UserState = {
    isSignedIn: false,
    userName: 'не авторизован',
    isLoading: false,
    isChecked: false,
    error: ''
}

const loadData = createAsyncThunk(
    'user',
    async (requestParams: RequestParams, thunkAPI) => {
        const response = await requestHelper.request(requestParams);
        if (response.success) {
            return thunkAPI.fulfillWithValue(response.data);
        } else {
            const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
            return thunkAPI.rejectWithValue(error);
        }
    }
)

export const userSlice = createSlice({
    name: 'user',
    initialState: defaultUserState,
    reducers: {},
    extraReducers: {
        [loadData.fulfilled.type]: (state, action: PayloadAction<IUserPrivate>) => {
            state.isSignedIn = action.payload ? true : false,
                state.userName = action.payload?.userName ?? 'не авторизован',
                state.isChecked = true,
                state.isLoading = false,
                state.error = ''
        },
        [loadData.pending.type]: (state) => {
            state.isChecked = true,
                state.isLoading = true,
                state.error = ''
        },
        [loadData.rejected.type]: (state, action: PayloadAction<string>) => {
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
        type: IRequestType.get,
        data: {  }
    }
    dispatch(loadData(requestParams));
}

const register = async (dispatch: AppDispatch,
    userName: string, password: string, passwordConfirm: string) => {
    
    const requestParams: RequestParams = {
        path: 'apiUser/register',
        type: IRequestType.post,
        data: { userName, password, passwordConfirm }
    }
    await dispatch(loadData(requestParams));
}

const login = async (dispatch: AppDispatch, userName: string, password: string) => {
    const requestParams: RequestParams = {
        path: 'apiUser/login',
        type: IRequestType.post,
        data: { userName, password }
    }
    await dispatch(loadData(requestParams));
}

const logout = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'apiUser/logout',
        type: IRequestType.post,
        data: {}
    }
    await dispatch(loadData(requestParams));
}

export const userActionCreators = { register, login, logout, getCurrentUser };
