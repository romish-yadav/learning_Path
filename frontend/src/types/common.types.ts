export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
  timestamp: string;
}

export interface PagedResponse<T> {
  success: boolean;
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
}

export interface SearchParams extends PaginationParams {
  query?: string;
  sortBy?: string;
  sortDescending?: boolean;
}

export interface PathFilterParams extends SearchParams {
  difficulty?: string;
  tags?: string;
  isPublished?: boolean;
}
