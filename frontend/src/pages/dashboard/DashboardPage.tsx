import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Card, CardContent, Typography, Box, LinearProgress } from '@mui/material';
import { School, CheckCircle, Whatshot, Star } from '@mui/icons-material';
import { fetchDashboard } from '../../redux/slices/dashboardSlice';
import type { AppDispatch, RootState } from '../../redux/store';
import { useAuth } from '../../hooks/useAuth';

const StatCard = ({ icon, label, value, color }: { icon: React.ReactNode; label: string; value: number | string; color: string }) => (
  <Card>
    <CardContent sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
      <Box sx={{ p: 1.5, borderRadius: 2, bgcolor: `${color}15`, color }}>{icon}</Box>
      <Box>
        <Typography variant="h4" sx={{ fontWeight: 700 }}>{value}</Typography>
        <Typography variant="body2" color="text.secondary">{label}</Typography>
      </Box>
    </CardContent>
  </Card>
);

const DashboardPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { user } = useAuth();
  const { data, loading } = useSelector((state: RootState) => state.dashboard);

  useEffect(() => {
    dispatch(fetchDashboard());
  }, [dispatch]);

  if (loading) return <LinearProgress />;

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Welcome back, {user?.firstName}!</Typography>
      <Grid container spacing={3} sx={{ mb: 4 }}>
        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
          <StatCard icon={<School />} label="Active Paths" value={data?.activePaths ?? 0} color="#1976d2" />
        </Grid>
        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
          <StatCard icon={<CheckCircle />} label="Completed Today" value={data?.completedToday ?? 0} color="#2e7d32" />
        </Grid>
        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
          <StatCard icon={<Whatshot />} label="Streak Days" value={data?.streakDays ?? 0} color="#ed6c02" />
        </Grid>
        <Grid size={{ xs: 12, sm: 6, md: 3 }}>
          <StatCard icon={<Star />} label="Total Points" value={data?.totalPoints ?? 0} color="#9c27b0" />
        </Grid>
      </Grid>

      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 8 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>In Progress</Typography>
              {data?.inProgressPaths?.length ? (
                data.inProgressPaths.map((path) => (
                  <Box key={path.pathId} sx={{ mb: 2 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 0.5 }}>
                      <Typography variant="body2">{path.pathTitle}</Typography>
                      <Typography variant="body2" color="text.secondary">{Math.round(path.completionPercentage)}%</Typography>
                    </Box>
                    <LinearProgress variant="determinate" value={path.completionPercentage} />
                  </Box>
                ))
              ) : (
                <Typography color="text.secondary">No paths in progress. Start exploring!</Typography>
              )}
            </CardContent>
          </Card>
        </Grid>
        <Grid size={{ xs: 12, md: 4 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>Recommended</Typography>
              {data?.recommendations?.length ? (
                data.recommendations.map((rec) => (
                  <Box key={rec.pathId} sx={{ mb: 1.5 }}>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>{rec.pathTitle}</Typography>
                    <Typography variant="caption" color="text.secondary">{rec.reason}</Typography>
                  </Box>
                ))
              ) : (
                <Typography color="text.secondary">Complete more modules to get recommendations.</Typography>
              )}
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
};

export default DashboardPage;
