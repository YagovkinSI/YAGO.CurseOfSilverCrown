import { Action, Reducer } from 'redux';

export interface UserState {
    isSignedIn: boolean,
    userName: string
}

export const actionCreators = {
};

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    if (state === undefined) {
        return { 
            isSignedIn: false,
            userName: ''
        };
    }

    return state;
};
