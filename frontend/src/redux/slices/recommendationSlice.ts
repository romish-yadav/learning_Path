import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import analyticsService from '../../services/analyticsService';
import type { Recommendation } from '../../types/analytics.types';

interface RecommendationState {
  recommendations: Recommendation[];
  loading: boolean;
  error: string | null;
}

const initialState: RecommendationState = {
  recommendations: [],
  loading: false,
  error: null,
};

export const fetchRecommendations = createAsyncThunk('recommendations/fetch', async (_, { rejectWithValue }) => {
  try {
    const response = await analyticsService.getRecommendations();
    return response.data.data!;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load recommendations');
  }
});

const recommendationSlice = createSlice({
  name: 'recommendations',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchRecommendations.pending, (state) => { state.loading = true; })
      .addCase(fetchRecommendations.fulfilled, (state, action) => {
        state.loading = false;
        state.recommendations = action.payload;
      })
      .addCase(fetchRecommendations.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default recommendationSlice.reducer;
