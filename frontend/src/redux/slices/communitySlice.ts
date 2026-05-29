import { createSlice } from '@reduxjs/toolkit';
import type { Comment, Rating } from '../../types/community.types';

interface CommunityState {
  comments: Comment[];
  ratings: Rating[];
  loading: boolean;
  error: string | null;
}

const initialState: CommunityState = {
  comments: [],
  ratings: [],
  loading: false,
  error: null,
};

const communitySlice = createSlice({
  name: 'community',
  initialState,
  reducers: {
    setComments: (state, action) => { state.comments = action.payload; },
    setRatings: (state, action) => { state.ratings = action.payload; },
    setLoading: (state, action) => { state.loading = action.payload; },
    setError: (state, action) => { state.error = action.payload; },
  },
});

export const { setComments, setRatings, setLoading, setError } = communitySlice.actions;
export default communitySlice.reducer;
