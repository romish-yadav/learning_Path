import type { PathProgress } from '../types/analytics.types';

export const calculateOverallProgress = (progresses: PathProgress[]): number => {
  if (progresses.length === 0) return 0;
  const total = progresses.reduce((sum, p) => sum + p.completionPercentage, 0);
  return Math.round(total / progresses.length);
};

export const formatHours = (minutes: number): string => {
  const h = Math.floor(minutes / 60);
  const m = minutes % 60;
  if (h === 0) return `${m}m`;
  if (m === 0) return `${h}h`;
  return `${h}h ${m}m`;
};
