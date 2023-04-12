import { PayloadAction, createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { AppDispatch, useAppSelector } from '.';
import { IRequestType, RequestParams, requestHelper } from '../helpers/RequestHelper';

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

const loadData = createAsyncThunk(
    'weatherForecast',
    async (requestParams: RequestParams, thunkAPI) => {
        const response = await requestHelper.request(requestParams);
        if (response.success) {
            return thunkAPI.fulfillWithValue(response.data);
        } else {
            const error = response.error == undefined ? 'Неизвестная ошибка' : response.error;
            return thunkAPI.rejectWithValue(error);
        }
    }
)

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
        [loadData.fulfilled.type]: (state, action: PayloadAction<WeatherForecast[]>) => {
            state.forecasts = action.payload,
                state.isLoading = false
        },
        [loadData.pending.type]: (state) => {
            state.forecasts = [],
                state.isLoading = true
        },
        [loadData.rejected.type]: (state, action: PayloadAction<string>) => {
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
        type: IRequestType.get,
        data: {}
    }
    return dispatch(loadData(requestParams));
}

export const weatherForecastsActionCreators = { requestWeatherForecasts };
