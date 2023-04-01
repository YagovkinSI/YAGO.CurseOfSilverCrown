import 'bootstrap/dist/css/bootstrap.css';

import React from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { BrowserRouter } from 'react-router-dom';
import { reducers } from './store';
import { configureStore } from '@reduxjs/toolkit';

const container = document.getElementById('root');
const root = createRoot(container!);
const appReducer = { reducer: { ...reducers } };
const store = configureStore(appReducer);

root.render(
    <Provider store={store}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </Provider>
);

registerServiceWorker();

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch