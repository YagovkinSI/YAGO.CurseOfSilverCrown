import { Action, Reducer } from 'redux';

export interface UserState {
    isSignedIn: boolean,
    userName: string
}

export const defaultUserState: UserState = {
    isSignedIn: false,
    userName: 'не авторизован'
}

export const actionCreators = {
};

export const reducer: Reducer<UserState> =
    (state: UserState = defaultUserState, incomingAction: Action): UserState => {
        return state;
    };
