import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
import { RequestType, requester, RequestParams } from "../requester";

export interface HistoryState {
    events?: string[][],
    isLoading: boolean,
    error: string
}

export const defaultHistoryState: HistoryState = {
    events: undefined,
    isLoading: false,
    error: ''
}

const request = requester.createThunk('history');

export const historySlice = createSlice({
    name: 'history',
    initialState: defaultHistoryState,
    reducers: {},
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<string[][]>) => {
            state.events = action.payload
            state.isLoading = false,
                state.error = ''
        },
        [request.pending.type]: (state) => {
            state.isLoading = true,
                state.error = ''
        },
        [request.rejected.type]: (state, action: PayloadAction<string>) => {
            state.isLoading = false,
                state.error = action.payload,
                state.error = action.payload
        }
    }
});

const getEvents = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'apiEvent/getEvents',
        type: RequestType.Get,
        data: {}
    }
    dispatch(request(requestParams));
}

export const historyActionCreators = { getEvents };
