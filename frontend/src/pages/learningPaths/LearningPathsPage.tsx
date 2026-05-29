import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Card, CardContent, CardMedia, Typography, Box, TextField, Select, MenuItem, Chip, Rating, Pagination } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { fetchPublicPaths } from '../../redux/slices/pathSlice';
import { useDebounce } from '../../hooks/useDebounce';
import type { AppDispatch, RootState } from '../../redux/store';
import { DIFFICULTY_OPTIONS } from '../../constants';

const LearningPathsPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { publicPaths, loading, totalCount } = useSelector((state: RootState) => state.paths);
  const [search, setSearch] = useState('');
  const [difficulty, setDifficulty] = useState('');
  const [page, setPage] = useState(1);
  const debouncedSearch = useDebounce(search, 300);

  useEffect(() => {
    dispatch(fetchPublicPaths({ page, pageSize: 12, query: debouncedSearch, difficulty: difficulty || undefined }));
  }, [dispatch, page, debouncedSearch, difficulty]);

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Explore Learning Paths</Typography>
      <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
        <TextField placeholder="Search paths..." value={search} onChange={(e) => setSearch(e.target.value)} size="small" sx={{ flex: 1 }} />
        <Select value={difficulty} onChange={(e) => setDifficulty(e.target.value)} displayEmpty size="small" sx={{ minWidth: 150 }}>
          <MenuItem value="">All Difficulties</MenuItem>
          {DIFFICULTY_OPTIONS.map((d) => <MenuItem key={d} value={d}>{d}</MenuItem>)}
        </Select>
      </Box>
      {loading ? (
        <Typography>Loading...</Typography>
      ) : (
        <>
          <Grid container spacing={3}>
            {publicPaths.map((path) => (
              <Grid size={{ xs: 12, sm: 6, md: 4 }} key={path.id}>
                <Card component={RouterLink} to={`/paths/${path.id}`} sx={{ textDecoration: 'none', height: '100%', display: 'flex', flexDirection: 'column' }}>
                  <CardMedia component="div" sx={{ height: 140, bgcolor: 'primary.light', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <Typography variant="h3" sx={{ color: 'white', opacity: 0.3 }}>LP</Typography>
                  </CardMedia>
                  <CardContent sx={{ flex: 1 }}>
                    <Typography variant="h6" gutterBottom>{path.title}</Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mb: 1, display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                      {path.description}
                    </Typography>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
                      <Chip label={path.difficulty} size="small" color="primary" variant="outlined" />
                      <Typography variant="caption" color="text.secondary">{path.moduleCount} modules</Typography>
                      <Typography variant="caption" color="text.secondary">~{path.estimatedHours}h</Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <Rating value={path.averageRating} readOnly size="small" precision={0.5} />
                      <Typography variant="caption" color="text.secondary">by {path.creatorName}</Typography>
                    </Box>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
          {totalCount > 12 && (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <Pagination count={Math.ceil(totalCount / 12)} page={page} onChange={(_, p) => setPage(p)} color="primary" />
            </Box>
          )}
        </>
      )}
    </Box>
  );
};

export default LearningPathsPage;
