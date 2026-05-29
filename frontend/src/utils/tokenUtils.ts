export const getToken = (): string | null => localStorage.getItem('token');

export const setToken = (token: string): void => localStorage.setItem('token', token);

export const removeToken = (): void => localStorage.removeItem('token');

export const getRefreshToken = (): string | null => localStorage.getItem('refreshToken');

export const setRefreshToken = (token: string): void => localStorage.setItem('refreshToken', token);

export const isTokenExpired = (token: string): boolean => {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
  } catch {
    return true;
  }
};
