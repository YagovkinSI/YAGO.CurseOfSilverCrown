import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
import { IRequestType, requestHelper, RequestParams } from "../helpers/RequestHelper";
import IUserPublicData from "../ApiModels/UserPublicData";

export interface UserState {
    user: IUserPublicData | undefined,
    isLoading: boolean,
    isChecked: boolean,
    error: string
}

export const defaultUserState: UserState = {
    user: undefined,
    isLoading: false,
    isChecked: false,
    error: ''
}

const loadData = createAsyncThunk(
    'user',
    async (requestParams: RequestParams, thunkAPI) => {
        const response = await requestHelper.request(requestParams);
        if (response.success) {
            const payload = response.data == ''
                ? undefined
                : response.data
            return thunkAPI.fulfillWithValue(payload);
        } else {
            const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
            return thunkAPI.rejectWithValue(error);
        }
    }
)

export const userSlice = createSlice({
    name: 'user',
    initialState: defaultUserState,
    reducers: { },
    extraReducers: {
        [loadData.fulfilled.type]: (state, action: PayloadAction<IUserPublicData>) => {
                state.user = action.payload
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
    dispatch(loadData(requestParams));
}

const login = async (dispatch: AppDispatch, userName: string, password: string) => {
    const requestParams: RequestParams = {
        path: 'userApi/login',
        type: IRequestType.post,
        data: { userName, password }
    }
    dispatch(loadData(requestParams));
}

const logout = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'userApi/logout',
        type: IRequestType.post,
        data: { }
    }
    await dispatch(loadData(requestParams));
}

export const userActionCreators = { register, login, logout };
