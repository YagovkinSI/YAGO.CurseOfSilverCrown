import { Action, Reducer } from 'redux';
import { AppThunkAction } from ".";

export interface UserState {
    isSignedIn: boolean,
    userName: string
}

export const defaultUserState: UserState = {
    isSignedIn: false,
    userName: 'не авторизован'
}

const setState = (state: UserState, action: SetState): UserState => {
    return {
        ...state,
        isSignedIn: action.isSignedIn,
        userName: action.userName
    }
}

interface SetState {
    type: 'User/SetState';
    isSignedIn: boolean,
    userName: string
}

type UserActions = SetState;

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
            default:
                return state;
        }
    };

const register = (userName: string, password: string, passwordConfirm: string)
    : AppThunkAction<UserActions> => async (dispatch, getState) => {
        dispatch({ type: 'User/SetState', isSignedIn: true, userName: userName });
        console.log('Выполнена эмуляция входа.');
    }

export const userActionCreators = { register };
