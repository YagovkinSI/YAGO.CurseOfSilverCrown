import { createSlice } from '@reduxjs/toolkit';
import { AppDispatch, useAppSelector } from '.';
import { IRequestType, requestHelper } from '../helpers/RequestHelper';


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

export const weatherForecastSlice = createSlice({
    name: 'weatherForecast',
    initialState: defaultWeatherForecastsState,
    reducers: {
        request(state, action) {
            state.startDateIndex = action.payload,
                state.isLoading = true,
                state.forecasts = []
        },
        recieve(state, action) {
            state.forecasts = action.payload,
                state.isLoading = false
        }
    }
});

const requestWeatherForecasts = async (dispatch: AppDispatch, startDateIndex: number) => {
    const state = useAppSelector(state => state.weatherForecastsReducer);
    if (state.isLoading || (state.startDateIndex == startDateIndex && state.forecasts.length > 0))
        return;

    console.log(state);
    dispatch(weatherForecastSlice.actions.request(startDateIndex));
    const response = await requestHelper.request('weatherforecast', IRequestType.get, {});
    dispatch(weatherForecastSlice.actions.recieve(response.data));
}

export const weatherForecastsActionCreators = { requestWeatherForecasts };
