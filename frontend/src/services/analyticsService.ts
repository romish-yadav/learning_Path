import apiClient from './apiClient';
import type { Dashboard, UserAnalytics, ClassroomAnalytics, Recommendation } from '../types/analytics.types';
import type { Module } from '../types/path.types';
import type { ApiResponse } from '../types/common.types';

const analyticsService = {
  getDashboard: () =>
    apiClient.get<ApiResponse<Dashboard>>('/analytics/dashboard'),

  getUserAnalytics: () =>
    apiClient.get<ApiResponse<UserAnalytics>>('/analytics/user'),

  getClassroomAnalytics: (classroomId: string) =>
    apiClient.get<ApiResponse<ClassroomAnalytics>>(`/analytics/classroom/${classroomId}`),

  getRecommendations: () =>
    apiClient.get<ApiResponse<Recommendation[]>>('/analytics/recommendations'),

  updateProgress: (moduleId: string, percentage: number) =>
    apiClient.post<ApiResponse<boolean>>(`/analytics/progress/${moduleId}?percentage=${percentage}`),

  completeModule: (moduleId: string) =>
    apiClient.post<ApiResponse<boolean>>(`/analytics/progress/${moduleId}/complete`),

  getUnlockedModules: (pathId: string) =>
    apiClient.get<ApiResponse<Module[]>>(`/analytics/progress/${pathId}/unlocked`),
};

export default analyticsService;
