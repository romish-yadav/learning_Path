import { createSlice } from '@reduxjs/toolkit';
import type { Notification } from '../../types/community.types';

interface NotificationState {
  notifications: Notification[];
  unreadCount: number;
  loading: boolean;
}

const initialState: NotificationState = {
  notifications: [],
  unreadCount: 0,
  loading: false,
};

const notificationSlice = createSlice({
  name: 'notifications',
  initialState,
  reducers: {
    setNotifications: (state, action) => { state.notifications = action.payload; },
    setUnreadCount: (state, action) => { state.unreadCount = action.payload; },
    markRead: (state, action) => {
      const notification = state.notifications.find(n => n.id === action.payload);
      if (notification) {
        notification.isRead = true;
        state.unreadCount = Math.max(0, state.unreadCount - 1);
      }
    },
    setLoading: (state, action) => { state.loading = action.payload; },
  },
});

export const { setNotifications, setUnreadCount, markRead, setLoading } = notificationSlice.actions;
export default notificationSlice.reducer;
