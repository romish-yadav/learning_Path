import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Card, CardContent, Typography, Box, LinearProgress } from '@mui/material';
import { fetchUserAnalytics } from '../../redux/slices/analyticsSlice';
import type { AppDispatch, RootState } from '../../redux/store';

const AnalyticsPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { userAnalytics, loading } = useSelector((state: RootState) => state.analytics);

  useEffect(() => {
    dispatch(fetchUserAnalytics());
  }, [dispatch]);

  if (loading) return <LinearProgress />;

  return (
    <Box>
      <Typography variant="h4" gutterBottom>My Analytics</Typography>
      <Grid container spacing={3} sx={{ mb: 4 }}>
        {[
          { label: 'Paths Enrolled', value: userAnalytics?.totalPathsEnrolled ?? 0 },
          { label: 'Paths Completed', value: userAnalytics?.pathsCompleted ?? 0 },
          { label: 'Modules Completed', value: userAnalytics?.modulesCompleted ?? 0 },
          { label: 'Certificates', value: userAnalytics?.certificatesEarned ?? 0 },
        ].map((stat) => (
          <Grid size={{ xs: 12, sm: 6, md: 3 }} key={stat.label}>
            <Card>
              <CardContent sx={{ textAlign: 'center' }}>
                <Typography variant="h3" color="primary" sx={{ fontWeight: 700 }}>{stat.value}</Typography>
                <Typography variant="body2" color="text.secondary">{stat.label}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Card>
        <CardContent>
          <Typography variant="h6" gutterBottom>Path Progress</Typography>
          {userAnalytics?.pathProgresses?.length ? (
            userAnalytics.pathProgresses.map((pp) => (
              <Box key={pp.pathId} sx={{ mb: 2 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 0.5 }}>
                  <Typography variant="body2">{pp.pathTitle}</Typography>
                  <Typography variant="body2" color="text.secondary">{pp.completedModules}/{pp.totalModules} modules</Typography>
                </Box>
                <LinearProgress variant="determinate" value={pp.completionPercentage} />
              </Box>
            ))
          ) : (
            <Typography color="text.secondary">Start a learning path to see your progress here.</Typography>
          )}
        </CardContent>
      </Card>
    </Box>
  );
};

export default AnalyticsPage;
