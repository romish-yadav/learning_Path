import apiClient from './apiClient';
import type { Recommendation } from '../types/analytics.types';
import type { ApiResponse } from '../types/common.types';

const recommendationService = {
  getRecommendations: () =>
    apiClient.get<ApiResponse<Recommendation[]>>('/analytics/recommendations'),
};

export default recommendationService;
