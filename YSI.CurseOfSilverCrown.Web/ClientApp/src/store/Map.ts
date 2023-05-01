import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
import { RequestType, requester, RequestParams } from "../requester";

export interface MapState {
    mapElements: IMapElement[] | undefined,
    isLoading: boolean,
    error: string
}

export interface IMapElement {
    id: number
    name: string
    colorKingdom: string
}

export const defaultMapState: MapState = {
    mapElements: undefined,
    isLoading: false,
    error: ''
}

const request = requester.createThunk('map');

export const mapSlice = createSlice({
    name: 'map',
    initialState: defaultMapState,
    reducers: {},
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<IMapElement[]>) => {
            state.mapElements = action.payload
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

const getMap = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'apiMap/getMap',
        type: RequestType.Get,
        data: {}
    }
    dispatch(request(requestParams));
}

export const mapActionCreators = { getMap };
