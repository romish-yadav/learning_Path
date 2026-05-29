import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import AppProvider from './app/provider';
import AppRoutes from './routes/AppRoutes';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AppProvider>
      <AppRoutes />
    </AppProvider>
  </StrictMode>,
);
