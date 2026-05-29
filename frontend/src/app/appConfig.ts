export const appConfig = {
  appName: 'LearnPath',
  version: '1.0.0',
  apiUrl: import.meta.env.VITE_API_URL || 'http://localhost:5000/api/v1',
  defaultPageSize: 10,
  tokenKey: 'token',
  refreshTokenKey: 'refreshToken',
};
