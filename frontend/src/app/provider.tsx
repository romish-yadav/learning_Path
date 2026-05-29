import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider, CssBaseline } from '@mui/material';
import { store } from '../redux/store';
import theme from '../theme';
import ErrorBoundary from '../components/common/ErrorBoundary/ErrorBoundary';
import type { ReactNode } from 'react';

interface AppProviderProps {
  children: ReactNode;
}

const AppProvider = ({ children }: AppProviderProps) => (
  <Provider store={store}>
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <ErrorBoundary>
          {children}
        </ErrorBoundary>
      </ThemeProvider>
    </BrowserRouter>
  </Provider>
);

export default AppProvider;
