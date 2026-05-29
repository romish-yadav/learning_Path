import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import dashboardReducer from './slices/dashboardSlice';
import pathReducer from './slices/pathSlice';
import classroomReducer from './slices/classroomSlice';
import analyticsReducer from './slices/analyticsSlice';
import communityReducer from './slices/communitySlice';
import notificationReducer from './slices/notificationSlice';
import recommendationReducer from './slices/recommendationSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    dashboard: dashboardReducer,
    paths: pathReducer,
    classrooms: classroomReducer,
    analytics: analyticsReducer,
    community: communityReducer,
    notifications: notificationReducer,
    recommendations: recommendationReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
