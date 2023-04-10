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

const requestRegiter = async (appState: ApplicationState,
    userName: string, password: string, passwordConfirm: string)
    : Promise<IResponse<boolean>> => {
    const apiPath = 'userApi/register';
    try {
        console.log(`request ${apiPath}`);
        const response = await axios.post(apiPath, { userName, password, passwordConfirm });
        console.log(`response ${apiPath}`, response);
        return {
            data: response.data,
            error: undefined,
            success: true
        } as IResponse<boolean>
    } catch (error) {
        console.log(`error ${apiPath}`, error);
        return {
            data: undefined,
            error: 'Произошла ошибка при выполнении регистрации',
            success: false
        } as IResponse<boolean>
    }
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

const register = (userName: string, password: string, passwordConfirm: string)
    : AppThunkAction<UserActions> => async (dispatch, getState) => {
        const appState = getState();
        if (appState.user.isLoading)
            return;
        dispatch({ type: 'User/SetLoading' })
        const response = await requestRegiter(appState, userName, password, passwordConfirm);
        if (response.success) {
            dispatch({ type: 'User/SetState', isSignedIn: true, userName });
        } else {
            const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
            dispatch({ type: 'User/SetError', error });
        }
    }

export const userActionCreators = { register };
