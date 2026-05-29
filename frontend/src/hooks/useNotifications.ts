import { useSelector } from 'react-redux';
import type { RootState } from '../redux/store';

export const useNotifications = () => {
  const { notifications, unreadCount, loading } = useSelector((state: RootState) => state.notifications);
  return { notifications, unreadCount, loading };
};
