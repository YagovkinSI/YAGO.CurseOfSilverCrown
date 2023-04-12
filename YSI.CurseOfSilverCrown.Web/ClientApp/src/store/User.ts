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
        setUserName(state, action) {
            state.userName = action.payload
        }
    },
    extraReducers: {
        [loadData.fulfilled.type]: (state, action: PayloadAction<boolean>) => {
            state.isSignedIn = action.payload,
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
    if (result.payload)
        dispatch(userSlice.actions.setUserName(userName));
}

const login = async (dispatch: AppDispatch, userName: string, password: string) => {
    const requestParams: RequestParams = {
        path: 'userApi/login',
        type: IRequestType.post,
        data: { userName, password }
    }
    const result = await dispatch(loadData(requestParams));
    if (result.payload)
        dispatch(userSlice.actions.setUserName(userName));
}

export const userActionCreators = { register, login };
