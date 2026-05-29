import apiClient from './apiClient';
import type {
  ClassroomSummary,
  ClassroomDetail,
  CreateClassroomRequest,
  UpdateClassroomRequest,
  AssignmentDetail,
  CreateAssignmentRequest,
  Submission,
  CreateSubmissionRequest,
  GradeSubmissionRequest,
} from '../types/classroom.types';
import type { ApiResponse, PagedResponse, PaginationParams } from '../types/common.types';

const classroomService = {
  getById: (id: string) =>
    apiClient.get<ApiResponse<ClassroomDetail>>(`/classrooms/${id}`),

  getMyClassrooms: (params: PaginationParams) =>
    apiClient.get<PagedResponse<ClassroomSummary>>('/classrooms/my', { params }),

  create: (data: CreateClassroomRequest) =>
    apiClient.post<ApiResponse<ClassroomDetail>>('/classrooms', data),

  update: (id: string, data: UpdateClassroomRequest) =>
    apiClient.put<ApiResponse<ClassroomDetail>>(`/classrooms/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<boolean>>(`/classrooms/${id}`),

  join: (joinCode: string) =>
    apiClient.post<ApiResponse<boolean>>('/classrooms/join', { joinCode }),

  leave: (id: string) =>
    apiClient.post<ApiResponse<boolean>>(`/classrooms/${id}/leave`),

  assignPath: (classroomId: string, pathId: string) =>
    apiClient.post<ApiResponse<boolean>>(`/classrooms/${classroomId}/paths/${pathId}`),

  createAssignment: (classroomId: string, data: CreateAssignmentRequest) =>
    apiClient.post<ApiResponse<AssignmentDetail>>(`/classrooms/${classroomId}/assignments`, data),

  submitAssignment: (assignmentId: string, data: CreateSubmissionRequest) =>
    apiClient.post<ApiResponse<Submission>>(`/classrooms/assignments/${assignmentId}/submit`, data),

  gradeSubmission: (submissionId: string, data: GradeSubmissionRequest) =>
    apiClient.post<ApiResponse<Submission>>(`/classrooms/submissions/${submissionId}/grade`, data),
};

export default classroomService;
