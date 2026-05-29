export const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api/v1';

export const ROLES = {
  ADMIN: 'Admin',
  INSTRUCTOR: 'Instructor',
  LEARNER: 'Learner',
} as const;

export const DIFFICULTY_OPTIONS = ['Beginner', 'Intermediate', 'Advanced', 'Expert'] as const;

export const MODULE_TYPES = ['Lesson', 'Quiz', 'Assignment', 'Video', 'Article', 'Project'] as const;

export const PAGE_SIZES = [5, 10, 20, 50] as const;

export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/register',
  DASHBOARD: '/dashboard',
  PATHS: '/paths',
  PATH_DETAIL: '/paths/:id',
  MY_PATHS: '/my-paths',
  CREATE_PATH: '/paths/create',
  CLASSROOMS: '/classrooms',
  CLASSROOM_DETAIL: '/classrooms/:id',
  ANALYTICS: '/analytics',
  COMMUNITY: '/community',
  CERTIFICATES: '/certificates',
  PROFILE: '/profile',
  SETTINGS: '/settings',
  NOTIFICATIONS: '/notifications',
  ADMIN: '/admin',
} as const;
