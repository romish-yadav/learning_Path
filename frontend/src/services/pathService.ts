import apiClient from './apiClient';
import type {
  LearningPathSummary,
  LearningPathDetail,
  CreateLearningPathRequest,
  UpdateLearningPathRequest,
  Module,
  CreateModuleRequest,
  UpdateModuleRequest,
} from '../types/path.types';
import type { ApiResponse, PagedResponse, PathFilterParams, PaginationParams } from '../types/common.types';

const pathService = {
  getPublicPaths: (params: PathFilterParams) =>
    apiClient.get<PagedResponse<LearningPathSummary>>('/paths', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<LearningPathDetail>>(`/paths/${id}`),

  getMyPaths: (params: PaginationParams) =>
    apiClient.get<PagedResponse<LearningPathSummary>>('/paths/my', { params }),

  create: (data: CreateLearningPathRequest) =>
    apiClient.post<ApiResponse<LearningPathDetail>>('/paths', data),

  update: (id: string, data: UpdateLearningPathRequest) =>
    apiClient.put<ApiResponse<LearningPathDetail>>(`/paths/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<boolean>>(`/paths/${id}`),

  publish: (id: string) =>
    apiClient.post<ApiResponse<boolean>>(`/paths/${id}/publish`),

  addModule: (pathId: string, data: CreateModuleRequest) =>
    apiClient.post<ApiResponse<Module>>(`/paths/${pathId}/modules`, data),

  updateModule: (pathId: string, moduleId: string, data: UpdateModuleRequest) =>
    apiClient.put<ApiResponse<Module>>(`/paths/${pathId}/modules/${moduleId}`, data),

  deleteModule: (pathId: string, moduleId: string) =>
    apiClient.delete<ApiResponse<boolean>>(`/paths/${pathId}/modules/${moduleId}`),
};

export default pathService;
