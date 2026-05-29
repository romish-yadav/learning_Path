import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';
import { CircularProgress, Box } from '@mui/material';
import MainLayout from '../layouts/MainLayout';
import AuthLayout from '../layouts/AuthLayout';
import DashboardLayout from '../layouts/DashboardLayout';
import ProtectedRoute from './ProtectedRoute';
import AdminRoute from './AdminRoute';
import GuestRoute from './GuestRoute';

const HomePage = lazy(() => import('../pages/public/HomePage'));
const LoginPage = lazy(() => import('../pages/auth/LoginPage'));
const RegisterPage = lazy(() => import('../pages/auth/RegisterPage'));
const DashboardPage = lazy(() => import('../pages/dashboard/DashboardPage'));
const LearningPathsPage = lazy(() => import('../pages/learningPaths/LearningPathsPage'));
const PathDetailPage = lazy(() => import('../pages/learningPaths/PathDetailPage'));
const MyPathsPage = lazy(() => import('../pages/learningPaths/MyPathsPage'));
const CreatePathPage = lazy(() => import('../pages/learningPaths/CreatePathPage'));
const ClassroomsPage = lazy(() => import('../pages/classroom/ClassroomsPage'));
const ClassroomDetailPage = lazy(() => import('../pages/classroom/ClassroomDetailPage'));
const AnalyticsPage = lazy(() => import('../pages/analytics/AnalyticsPage'));
const CommunityPage = lazy(() => import('../pages/community/CommunityPage'));
const CertificatesPage = lazy(() => import('../pages/certificates/CertificatesPage'));
const ProfilePage = lazy(() => import('../pages/profile/ProfilePage'));
const SettingsPage = lazy(() => import('../pages/settings/SettingsPage'));
const NotificationsPage = lazy(() => import('../pages/notifications/NotificationsPage'));
const AdminDashboard = lazy(() => import('../pages/admin/AdminDashboard'));
const NotFoundPage = lazy(() => import('../pages/errors/NotFoundPage'));

const Loading = () => (
  <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
    <CircularProgress />
  </Box>
);

const AppRoutes = () => (
  <Suspense fallback={<Loading />}>
    <Routes>
      {/* Public routes */}
      <Route element={<MainLayout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/paths" element={<LearningPathsPage />} />
        <Route path="/paths/:id" element={<PathDetailPage />} />
      </Route>

      {/* Auth routes (guest only) */}
      <Route element={<GuestRoute />}>
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>
      </Route>

      {/* Protected routes */}
      <Route element={<ProtectedRoute />}>
        <Route element={<DashboardLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/my-paths" element={<MyPathsPage />} />
          <Route path="/paths/create" element={<CreatePathPage />} />
          <Route path="/classrooms" element={<ClassroomsPage />} />
          <Route path="/classrooms/:id" element={<ClassroomDetailPage />} />
          <Route path="/analytics" element={<AnalyticsPage />} />
          <Route path="/community" element={<CommunityPage />} />
          <Route path="/certificates" element={<CertificatesPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/settings" element={<SettingsPage />} />
          <Route path="/notifications" element={<NotificationsPage />} />
        </Route>
      </Route>

      {/* Admin routes */}
      <Route element={<AdminRoute />}>
        <Route element={<DashboardLayout />}>
          <Route path="/admin" element={<AdminDashboard />} />
        </Route>
      </Route>

      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  </Suspense>
);

export default AppRoutes;
