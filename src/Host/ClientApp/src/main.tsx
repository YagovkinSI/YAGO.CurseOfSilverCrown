import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import { CssBaseline } from '@mui/material';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { setupStore } from './AppStore.ts';

import './index.css'

const theme = createTheme();
const store = setupStore();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Provider store={store}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </Provider>
    </ThemeProvider>
  </StrictMode>,
)