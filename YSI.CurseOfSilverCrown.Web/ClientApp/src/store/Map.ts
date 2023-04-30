import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { AppDispatch } from ".";
import { IRequestType, requestHelper, RequestParams } from "../helpers/RequestHelper";
import IMapElement from "../apiModels/mapElement";

export interface MapState {
    mapElements: IMapElement[] | undefined,
    isLoading: boolean,
    error: string
}

export const defaultMapState: MapState = {
    mapElements: undefined,
    isLoading: false,
    error: ''
}

const loadData = requestHelper.createThunk('map');

export const mapSlice = createSlice({
    name: 'map',
    initialState: defaultMapState,
    reducers: {},
    extraReducers: {
        [loadData.fulfilled.type]: (state, action: PayloadAction<IMapElement[]>) => {
            state.mapElements = action.payload
            state.isLoading = false,
                state.error = ''
        },
        [loadData.pending.type]: (state) => {
            state.isLoading = true,
                state.error = ''
        },
        [loadData.rejected.type]: (state, action: PayloadAction<string>) => {
            state.isLoading = false,
                state.error = action.payload,
                state.error = action.payload
        }
    }
});

const getMap = async (dispatch: AppDispatch) => {
    const requestParams: RequestParams = {
        path: 'apiMap/getMap',
        type: IRequestType.get,
        data: {}
    }
    dispatch(loadData(requestParams));
}

export const mapActionCreators = { getMap };
