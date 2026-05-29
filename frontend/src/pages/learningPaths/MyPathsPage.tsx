import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Card, CardContent, Typography, Box, Button, Chip } from '@mui/material';
import { Add } from '@mui/icons-material';
import { Link as RouterLink } from 'react-router-dom';
import { fetchMyPaths } from '../../redux/slices/pathSlice';
import type { AppDispatch, RootState } from '../../redux/store';

const MyPathsPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { myPaths } = useSelector((state: RootState) => state.paths);

  useEffect(() => {
    dispatch(fetchMyPaths({ page: 1, pageSize: 20 }));
  }, [dispatch]);

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4">My Learning Paths</Typography>
        <Button variant="contained" startIcon={<Add />} component={RouterLink} to="/paths/create">Create Path</Button>
      </Box>
      <Grid container spacing={3}>
        {myPaths.map((path) => (
          <Grid size={{ xs: 12, sm: 6, md: 4 }} key={path.id}>
            <Card component={RouterLink} to={`/paths/${path.id}`} sx={{ textDecoration: 'none' }}>
              <CardContent>
                <Typography variant="h6">{path.title}</Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>{path.description}</Typography>
                <Box sx={{ display: 'flex', gap: 1 }}>
                  <Chip label={path.difficulty} size="small" />
                  <Chip label={path.isPublished ? 'Published' : 'Draft'} size="small" color={path.isPublished ? 'success' : 'default'} />
                  <Chip label={`${path.moduleCount} modules`} size="small" variant="outlined" />
                </Box>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
      {myPaths.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">You haven't created any learning paths yet.</Typography>
          <Button variant="contained" sx={{ mt: 2 }} component={RouterLink} to="/paths/create">Create Your First Path</Button>
        </Box>
      )}
    </Box>
  );
};

export default MyPathsPage;
