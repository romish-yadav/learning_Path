import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import classroomService from '../../services/classroomService';
import type { ClassroomSummary, ClassroomDetail } from '../../types/classroom.types';
import type { PaginationParams } from '../../types/common.types';

interface ClassroomState {
  classrooms: ClassroomSummary[];
  currentClassroom: ClassroomDetail | null;
  loading: boolean;
  error: string | null;
}

const initialState: ClassroomState = {
  classrooms: [],
  currentClassroom: null,
  loading: false,
  error: null,
};

export const fetchMyClassrooms = createAsyncThunk('classrooms/fetchMy', async (params: PaginationParams, { rejectWithValue }) => {
  try {
    const response = await classroomService.getMyClassrooms(params);
    return response.data;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load classrooms');
  }
});

export const fetchClassroomById = createAsyncThunk('classrooms/fetchById', async (id: string, { rejectWithValue }) => {
  try {
    const response = await classroomService.getById(id);
    return response.data.data!;
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    return rejectWithValue(error.response?.data?.message || 'Failed to load classroom');
  }
});

const classroomSlice = createSlice({
  name: 'classrooms',
  initialState,
  reducers: {
    clearCurrentClassroom: (state) => { state.currentClassroom = null; },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchMyClassrooms.pending, (state) => { state.loading = true; })
      .addCase(fetchMyClassrooms.fulfilled, (state, action) => {
        state.loading = false;
        state.classrooms = action.payload.data;
      })
      .addCase(fetchMyClassrooms.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchClassroomById.pending, (state) => { state.loading = true; })
      .addCase(fetchClassroomById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentClassroom = action.payload;
      })
      .addCase(fetchClassroomById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { clearCurrentClassroom } = classroomSlice.actions;
export default classroomSlice.reducer;
