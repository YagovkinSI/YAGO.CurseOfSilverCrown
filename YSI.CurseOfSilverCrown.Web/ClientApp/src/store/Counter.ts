import { createSlice } from '@reduxjs/toolkit';
import { AppDispatch } from '.';

export interface CounterState {
    count: number;
}

export const defaultCounterState: CounterState = {
    count: 0
}

export const сounterSlice = createSlice({
    name: 'сounter',
    initialState: defaultCounterState,
    reducers: {
        increment(state) {
            state.count++
        },
        decrement(state) {
            state.count--
        }
    }
});

const increment = (dispatch: AppDispatch) => {
    dispatch(сounterSlice.actions.increment())
}

const decrement = (dispatch: AppDispatch) => {
    dispatch(сounterSlice.actions.decrement())
}

export const counterActionCreators = { increment, decrement };
