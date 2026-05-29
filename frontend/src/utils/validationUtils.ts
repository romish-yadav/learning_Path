export const isValidEmail = (email: string): boolean =>
  /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

export const isValidPassword = (password: string): boolean =>
  password.length >= 6 &&
  /[A-Z]/.test(password) &&
  /[a-z]/.test(password) &&
  /[0-9]/.test(password) &&
  /[^A-Za-z0-9]/.test(password);

export const isNotEmpty = (value: string): boolean =>
  value.trim().length > 0;

export const isWithinLength = (value: string, min: number, max: number): boolean =>
  value.length >= min && value.length <= max;
