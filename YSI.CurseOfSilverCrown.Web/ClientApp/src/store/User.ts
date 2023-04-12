import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
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

interface IUserAction {
    isSignedIn: boolean,
    userName: string
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
    reducers: {
        setState(state, action: PayloadAction<IUserAction>) {
            state.isSignedIn = action.payload.isSignedIn,
            state.userName = action.payload.userName
        }
    },
    extraReducers: {
        [loadData.fulfilled.type]: (state) => {
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

const register = async (dispatch: AppDispatch,
    userName: string, password: string, passwordConfirm: string) => {
    
    const requestParams: RequestParams = {
        path: 'userApi/register',
        type: IRequestType.post,
        data: { userName, password, passwordConfirm }
    }
    const result = await dispatch(loadData(requestParams));
    if (result.meta.requestStatus == "fulfilled")
        dispatch(userSlice.actions.setState({ isSignedIn: true, userName}));
}

const login = async (dispatch: AppDispatch, userName: string, password: string) => {
    const requestParams: RequestParams = {
        path: 'userApi/login',
        type: IRequestType.post,
        data: { userName, password }
    }
    const result = await dispatch(loadData(requestParams));
    if (result.meta.requestStatus == "fulfilled")
        dispatch(userSlice.actions.setState({ isSignedIn: true, userName}));
}

const logout = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'userApi/logout',
        type: IRequestType.post,
        data: { }
    }
    const result = await dispatch(loadData(requestParams));
    if (result.meta.requestStatus == "fulfilled")
        dispatch(userSlice.actions.setState({ isSignedIn: false, userName:'не авторизован'}));
}

export const userActionCreators = { register, login, logout };
