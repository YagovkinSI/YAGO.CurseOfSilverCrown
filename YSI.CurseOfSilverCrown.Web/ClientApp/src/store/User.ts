import { createSlice } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
import { IRequestType, requestHelper } from "../helpers/RequestHelper";

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

export const userSlice = createSlice({
    name: 'user',
    initialState: defaultUserState,
    reducers: {
        setState(state, action) {
            state.isSignedIn = action.payload.isSignedIn,
                state.userName = action.payload.userName,
                state.isChecked = true,
                state.isLoading = false,
                state.error = ''
        },
        setLoading(state) {
            state.isChecked = true,
                state.isLoading = true,
                state.error = ''
        },
        setError(state, action) {
            state.isChecked = true,
                state.isLoading = false,
                state.error = action.payload
        }
    }
});

const loadDataFromServer = async (dispatch: AppDispatch, apiPath: string, data: any) => {
    dispatch(userSlice.actions.setLoading());
    const response = await requestHelper.request(apiPath, IRequestType.post, data);
    if (response.success) {
        dispatch(userSlice.actions.setState({
            isSignedIn: response.data == null ? false : true,
            userName: response.data?.userName ?? 'не авторизован'
        }));
    } else {
        const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
        dispatch(userSlice.actions.setError(error));
    }
}

const register = (dispatch: AppDispatch,
    userName: string, password: string, passwordConfirm: string) => {
    loadDataFromServer(dispatch, 'apiUser/register',
        { userName, password, passwordConfirm })
}

const login = (dispatch: AppDispatch, userName: string, password: string) => {
    loadDataFromServer(dispatch, 'apiUser/login', { userName, password })
}

export const userActionCreators = { register, login };
