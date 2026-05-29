import apiClient from './apiClient';
import type { User } from '../types/auth.types';
import type { ApiResponse, PagedResponse, SearchParams } from '../types/common.types';

const adminService = {
  getUsers: (params: SearchParams) =>
    apiClient.get<PagedResponse<User>>('/admin/users', { params }),

  assignRole: (userId: string, role: string) =>
    apiClient.post<ApiResponse<boolean>>(`/admin/users/${userId}/role`, JSON.stringify(role), {
      headers: { 'Content-Type': 'application/json' },
    }),

  removeRole: (userId: string, role: string) =>
    apiClient.delete<ApiResponse<boolean>>(`/admin/users/${userId}/role`, {
      data: JSON.stringify(role),
      headers: { 'Content-Type': 'application/json' },
    }),

  getStats: () =>
    apiClient.get<ApiResponse<{
      totalUsers: number;
      totalPaths: number;
      totalClassrooms: number;
      totalModules: number;
      publishedPaths: number;
    }>>('/admin/stats'),
};

export default adminService;
