import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { AppDispatch, useAppSelector } from '.';
import { RequestType, RequestParams, requester } from '../requester';

export interface WeatherForecastsState {
    isLoading: boolean;
    startDateIndex: number;
    forecasts: WeatherForecast[];
}

export const defaultWeatherForecastsState: WeatherForecastsState = {
    isLoading: false,
    startDateIndex: 0,
    forecasts: []
}

export interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

const request = requester.createThunk('weatherForecast')

export const weatherForecastSlice = createSlice({
    name: 'weatherForecast',
    initialState: defaultWeatherForecastsState,
    reducers: {
        setStartDateIndex(state, action: PayloadAction<number>) {
            state.forecasts = [],
            state.startDateIndex = action.payload
        }
    },
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<WeatherForecast[]>) => {
            state.forecasts = action.payload,
                state.isLoading = false
        },
        [request.pending.type]: (state) => {
            state.forecasts = [],
                state.isLoading = true
        },
        [request.rejected.type]: (state, action: PayloadAction<string>) => {
            state.forecasts = [],
                state.isLoading = false
        }
    }
});

const requestWeatherForecasts = async (dispatch: AppDispatch, startDateIndex: number) => {
    const state = useAppSelector(state => state.weatherForecastsReducer);
    if (state.isLoading || (state.startDateIndex == startDateIndex && state.forecasts.length > 0))
        return;

    dispatch(weatherForecastSlice.actions.setStartDateIndex(startDateIndex));
    const requestParams: RequestParams = {
        path: 'weatherforecast',
        type: RequestType.Get,
        data: {}
    }
    return dispatch(request(requestParams));
}

export const weatherForecastsActionCreators = { requestWeatherForecasts };
