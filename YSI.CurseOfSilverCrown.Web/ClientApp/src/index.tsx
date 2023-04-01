import 'bootstrap/dist/css/bootstrap.css';

import React from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { BrowserRouter } from 'react-router-dom';
import { setupStore } from './store';

const container = document.getElementById('root');
const root = createRoot(container!);
const store = setupStore()

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