import { useSelector, useDispatch } from 'react-redux';
import type { RootState, AppDispatch } from '../redux/store';
import { logout } from '../redux/slices/authSlice';

export const useAuth = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { user, isAuthenticated, loading, error } = useSelector((state: RootState) => state.auth);

  const handleLogout = () => {
    dispatch(logout());
  };

  const isAdmin = user?.roles.includes('Admin') ?? false;
  const isInstructor = user?.roles.includes('Instructor') ?? false;

  return { user, isAuthenticated, loading, error, isAdmin, isInstructor, logout: handleLogout };
};
