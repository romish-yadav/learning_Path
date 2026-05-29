import apiClient from './apiClient';
import type {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  RefreshTokenRequest,
  UpdateProfileRequest,
  ChangePasswordRequest,
  User,
} from '../types/auth.types';
import type { ApiResponse } from '../types/common.types';

const authService = {
  register: (data: RegisterRequest) =>
    apiClient.post<ApiResponse<AuthResponse>>('/auth/register', data),

  login: (data: LoginRequest) =>
    apiClient.post<ApiResponse<AuthResponse>>('/auth/login', data),

  refreshToken: (data: RefreshTokenRequest) =>
    apiClient.post<ApiResponse<AuthResponse>>('/auth/refresh-token', data),

  revokeToken: () =>
    apiClient.post<ApiResponse<boolean>>('/auth/revoke-token'),

  getProfile: () =>
    apiClient.get<ApiResponse<User>>('/auth/profile'),

  updateProfile: (data: UpdateProfileRequest) =>
    apiClient.put<ApiResponse<User>>('/auth/profile', data),

  changePassword: (data: ChangePasswordRequest) =>
    apiClient.post<ApiResponse<boolean>>('/auth/change-password', data),
};

export default authService;
