import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { Box, Typography, Card, CardContent, Grid, Chip, List, ListItem, ListItemAvatar, Avatar, ListItemText, Divider, LinearProgress } from '@mui/material';
import { fetchClassroomById } from '../../redux/slices/classroomSlice';
import { formatDate } from '../../utils/dateUtils';
import type { AppDispatch, RootState } from '../../redux/store';

const ClassroomDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { currentClassroom, loading } = useSelector((state: RootState) => state.classrooms);

  useEffect(() => {
    if (id) dispatch(fetchClassroomById(id));
  }, [dispatch, id]);

  if (loading) return <LinearProgress />;
  if (!currentClassroom) return <Typography>Classroom not found.</Typography>;

  return (
    <Box>
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" gutterBottom>{currentClassroom.name}</Typography>
        <Typography variant="body1" color="text.secondary">{currentClassroom.description}</Typography>
        {currentClassroom.joinCode && (
          <Chip label={`Join Code: ${currentClassroom.joinCode}`} sx={{ mt: 1 }} color="primary" />
        )}
      </Box>

      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 8 }}>
          <Card sx={{ mb: 3 }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Assignments</Typography>
              {currentClassroom.assignments.length > 0 ? (
                <List>
                  {currentClassroom.assignments.map((a) => (
                    <ListItem key={a.id}>
                      <ListItemText
                        primary={a.title}
                        secondary={`Due: ${a.dueDate ? formatDate(a.dueDate) : 'No deadline'} | ${a.submissionCount} submissions | Max: ${a.maxScore} pts`}
                      />
                    </ListItem>
                  ))}
                </List>
              ) : (
                <Typography color="text.secondary">No assignments yet.</Typography>
              )}
            </CardContent>
          </Card>
        </Grid>

        <Grid size={{ xs: 12, md: 4 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>Members ({currentClassroom.members.length})</Typography>
              <List>
                {currentClassroom.members.map((m, i) => (
                  <Box key={m.userId}>
                    {i > 0 && <Divider />}
                    <ListItem>
                      <ListItemAvatar><Avatar>{m.name[0]}</Avatar></ListItemAvatar>
                      <ListItemText primary={m.name} secondary={<Chip label={m.role} size="small" />} />
                    </ListItem>
                  </Box>
                ))}
              </List>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
};

export default ClassroomDetailPage;
