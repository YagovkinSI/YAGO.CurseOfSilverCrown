import axios from "axios";
import { Action, Reducer } from 'redux';
import { ApplicationState, AppThunkAction } from ".";

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

const setState = (state: UserState, action: SetState): UserState => {
    return {
        ...state,
        isSignedIn: action.isSignedIn,
        userName: action.userName,
        isChecked: true,
        isLoading: false,
        error: ''
    }
}

const setLoading = (state: UserState): UserState => {
    return {
        ...state,
        isChecked: true,
        isLoading: true,
        error: ''
    };
}

const setError = (state: UserState, errorMessage: string): UserState => {
    return {
        ...state,
        isChecked: true,
        isLoading: false,
        error: errorMessage
    };
}

interface SetState {
    type: 'User/SetState';
    isSignedIn: boolean,
    userName: string
}

interface SetLoading {
    type: 'User/SetLoading';
}

interface SetError {
    type: 'User/SetError';
    error: string
}

type UserActions = SetState | SetLoading | SetError;

interface IResponse<T> {
    data: T | undefined,
    error: string | undefined,
    success: boolean
}

const request = async (apiPath: string, data: any)
    : Promise<IResponse<any>> => {
    try {
        console.log(`request ${apiPath}`);
        const response = await axios.post(apiPath, data);
        console.log(`response ${apiPath}`, response);
        return {
            data: response.data,
            error: undefined,
            success: true
        } as IResponse<any>
    } catch (error) {
        console.log(`error ${apiPath}`, error);
        return {
            data: undefined,
            error: 'Произошла неизвестная ошибка',
            success: false
        } as IResponse<any>
    }
}

const loadDataFromServer = (apiPath: string, data: any)
    : AppThunkAction<UserActions> => async (dispatch, getState) => {
        const appState = getState();
        if (appState.user.isLoading)
            return;
        dispatch({ type: 'User/SetLoading' })
        const response = await request(apiPath, data);
        if (response.success) {
            dispatch({ 
                type: 'User/SetState', 
                isSignedIn: response.data == null ? false : true, 
                userName: response.data?.userName ?? 'не авторизован'
            });
        } else {
            const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
            dispatch({ type: 'User/SetError', error });
        }
    }

const register = (userName: string, password: string, passwordConfirm: string) => {
    return loadDataFromServer('apiUser/register', { userName, password, passwordConfirm })
}

const login = (userName: string, password: string) => {
    return loadDataFromServer('apiUser/login', { userName, password });
}

export const actionCreators = {
};

export const reducer: Reducer<UserState> =
    (state: UserState = defaultUserState, incomingAction: Action): UserState => {
        const action = incomingAction as UserActions;
        if (action == undefined)
            return state;

        switch (action.type) {
            case 'User/SetState':
                return setState(state, action);
            case 'User/SetLoading':
                return setLoading(state);
            case 'User/SetError':
                return setError(state, action.error);
            default:
                return state;
        }
    };

export const userActionCreators = { register, login };
