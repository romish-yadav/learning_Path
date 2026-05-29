import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import pathService from '../../services/pathService';
import type { LearningPathSummary, LearningPathDetail } from '../../types/path.types';
import type { PathFilterParams, PaginationParams } from '../../types/common.types';

interface PathState {
  publicPaths: LearningPathSummary[];
  myPaths: LearningPathSummary[];
  currentPath: LearningPathDetail | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
}

const initialState: PathState = {
  publicPaths: [],
  myPaths: [],
  currentPath: null,
  loading: false,
  error: null,
  totalCount: 0,
};

export const fetchPublicPaths = createAsyncThunk('paths/fetchPublic', async (params: PathFilterParams, { rejectWithValue }) => {
  try {
    const response = await pathService.getPublicPaths(params);
    return response.data;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load paths');
  }
});

export const fetchMyPaths = createAsyncThunk('paths/fetchMy', async (params: PaginationParams, { rejectWithValue }) => {
  try {
    const response = await pathService.getMyPaths(params);
    return response.data;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load paths');
  }
});

export const fetchPathById = createAsyncThunk('paths/fetchById', async (id: string, { rejectWithValue }) => {
  try {
    const response = await pathService.getById(id);
    return response.data.data!;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load path');
  }
});

const pathSlice = createSlice({
  name: 'paths',
  initialState,
  reducers: {
    clearCurrentPath: (state) => { state.currentPath = null; },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchPublicPaths.pending, (state) => { state.loading = true; })
      .addCase(fetchPublicPaths.fulfilled, (state, action) => {
        state.loading = false;
        state.publicPaths = action.payload.data;
        state.totalCount = action.payload.totalCount;
      })
      .addCase(fetchPublicPaths.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchMyPaths.fulfilled, (state, action) => {
        state.myPaths = action.payload.data;
      })
      .addCase(fetchPathById.pending, (state) => { state.loading = true; })
      .addCase(fetchPathById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentPath = action.payload;
      })
      .addCase(fetchPathById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { clearCurrentPath } = pathSlice.actions;
export default pathSlice.reducer;
