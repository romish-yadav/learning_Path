import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import analyticsService from '../../services/analyticsService';
import type { UserAnalytics } from '../../types/analytics.types';

interface AnalyticsState {
  userAnalytics: UserAnalytics | null;
  loading: boolean;
  error: string | null;
}

const initialState: AnalyticsState = {
  userAnalytics: null,
  loading: false,
  error: null,
};

export const fetchUserAnalytics = createAsyncThunk('analytics/fetchUser', async (_, { rejectWithValue }) => {
  try {
    const response = await analyticsService.getUserAnalytics();
    return response.data.data!;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load analytics');
  }
});

const analyticsSlice = createSlice({
  name: 'analytics',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchUserAnalytics.pending, (state) => { state.loading = true; })
      .addCase(fetchUserAnalytics.fulfilled, (state, action) => {
        state.loading = false;
        state.userAnalytics = action.payload;
      })
      .addCase(fetchUserAnalytics.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default analyticsSlice.reducer;
