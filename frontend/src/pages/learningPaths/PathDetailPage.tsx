import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { Box, Typography, Chip, Rating, Card, CardContent, List, ListItem, ListItemIcon, ListItemText, LinearProgress, Divider } from '@mui/material';
import { PlayCircle, Quiz, Assignment, VideoLibrary, Article, Code, Lock, LockOpen } from '@mui/icons-material';
import { fetchPathById } from '../../redux/slices/pathSlice';
import type { AppDispatch, RootState } from '../../redux/store';
import type { ModuleType } from '../../types/path.types';

const moduleIcons: Record<ModuleType, React.ReactNode> = {
  Lesson: <PlayCircle />,
  Quiz: <Quiz />,
  Assignment: <Assignment />,
  Video: <VideoLibrary />,
  Article: <Article />,
  Project: <Code />,
};

const PathDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { currentPath, loading } = useSelector((state: RootState) => state.paths);

  useEffect(() => {
    if (id) dispatch(fetchPathById(id));
  }, [dispatch, id]);

  if (loading) return <LinearProgress />;
  if (!currentPath) return <Typography>Path not found.</Typography>;

  return (
    <Box>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h3" gutterBottom>{currentPath.title}</Typography>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
          <Chip label={currentPath.difficulty} color="primary" />
          <Typography variant="body2" color="text.secondary">by {currentPath.creatorName}</Typography>
          <Typography variant="body2" color="text.secondary">~{currentPath.estimatedHours} hours</Typography>
          <Typography variant="body2" color="text.secondary">{currentPath.moduleCount} modules</Typography>
        </Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
          <Rating value={currentPath.averageRating} readOnly precision={0.5} />
          <Typography variant="body2" color="text.secondary">({currentPath.ratingCount} ratings)</Typography>
        </Box>
        <Typography variant="body1" color="text.secondary">{currentPath.description}</Typography>
        {currentPath.tags && (
          <Box sx={{ mt: 2, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
            {currentPath.tags.split(',').map((tag) => (
              <Chip key={tag.trim()} label={tag.trim()} size="small" variant="outlined" />
            ))}
          </Box>
        )}
      </Box>

      <Typography variant="h5" gutterBottom>Modules</Typography>
      <Card>
        <List>
          {currentPath.modules.map((module, index) => (
            <Box key={module.id}>
              {index > 0 && <Divider />}
              <ListItem>
                <ListItemIcon>{moduleIcons[module.type] || <PlayCircle />}</ListItemIcon>
                <ListItemText
                  primary={`${module.orderIndex + 1}. ${module.title}`}
                  secondary={
                    <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
                      <Chip label={module.type} size="small" variant="outlined" />
                      <Typography variant="caption">{module.estimatedMinutes} min</Typography>
                      {module.prerequisiteIds.length > 0 ? <Lock fontSize="small" color="action" /> : <LockOpen fontSize="small" color="success" />}
                    </Box>
                  }
                />
              </ListItem>
            </Box>
          ))}
        </List>
      </Card>
    </Box>
  );
};

export default PathDetailPage;
