import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { сounterSlice } from "./Counter";
import { weatherForecastSlice } from "./WeatherForecasts";
import { userSlice } from "./User";
import { mapSlice } from "./Map";
import { activeDomainSlice } from "./ActiveDomain";

const rootReducer = combineReducers({
    counterReducer: сounterSlice.reducer,
    weatherForecastsReducer: weatherForecastSlice.reducer,
    userReducer: userSlice.reducer,
    mapReducer: mapSlice.reducer,
    activeDomainReducer: activeDomainSlice.reducer
})

export const setupStore = () => {
    return configureStore({
        reducer: rootReducer
    })
}

export type RootState = ReturnType<typeof rootReducer>
export type AppStore = ReturnType<typeof setupStore>
export type AppDispatch = AppStore['dispatch']

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
