import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Card, CardContent, Typography, Box, Button, Chip, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';
import { Add, Login } from '@mui/icons-material';
import { Link as RouterLink } from 'react-router-dom';
import { fetchMyClassrooms } from '../../redux/slices/classroomSlice';
import classroomService from '../../services/classroomService';
import type { AppDispatch, RootState } from '../../redux/store';

const ClassroomsPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { classrooms } = useSelector((state: RootState) => state.classrooms);
  const [joinDialog, setJoinDialog] = useState(false);
  const [createDialog, setCreateDialog] = useState(false);
  const [joinCode, setJoinCode] = useState('');
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');

  useEffect(() => {
    dispatch(fetchMyClassrooms({ page: 1, pageSize: 20 }));
  }, [dispatch]);

  const handleJoin = async () => {
    await classroomService.join(joinCode);
    setJoinDialog(false);
    setJoinCode('');
    dispatch(fetchMyClassrooms({ page: 1, pageSize: 20 }));
  };

  const handleCreate = async () => {
    await classroomService.create({ name, description });
    setCreateDialog(false);
    setName(''); setDescription('');
    dispatch(fetchMyClassrooms({ page: 1, pageSize: 20 }));
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">My Classrooms</Typography>
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Button variant="outlined" startIcon={<Login />} onClick={() => setJoinDialog(true)}>Join</Button>
          <Button variant="contained" startIcon={<Add />} onClick={() => setCreateDialog(true)}>Create</Button>
        </Box>
      </Box>
      <Grid container spacing={3}>
        {classrooms.map((c) => (
          <Grid size={{ xs: 12, sm: 6, md: 4 }} key={c.id}>
            <Card component={RouterLink} to={`/classrooms/${c.id}`} sx={{ textDecoration: 'none' }}>
              <CardContent>
                <Typography variant="h6">{c.name}</Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>{c.description}</Typography>
                <Box sx={{ display: 'flex', gap: 1 }}>
                  <Chip label={`${c.studentCount} students`} size="small" />
                  <Chip label={`${c.pathCount} paths`} size="small" variant="outlined" />
                  <Chip label={c.isActive ? 'Active' : 'Inactive'} size="small" color={c.isActive ? 'success' : 'default'} />
                </Box>
                <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>Instructor: {c.instructorName}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Dialog open={joinDialog} onClose={() => setJoinDialog(false)}>
        <DialogTitle>Join Classroom</DialogTitle>
        <DialogContent><TextField autoFocus fullWidth label="Join Code" value={joinCode} onChange={(e) => setJoinCode(e.target.value)} sx={{ mt: 1 }} /></DialogContent>
        <DialogActions><Button onClick={() => setJoinDialog(false)}>Cancel</Button><Button variant="contained" onClick={handleJoin}>Join</Button></DialogActions>
      </Dialog>

      <Dialog open={createDialog} onClose={() => setCreateDialog(false)}>
        <DialogTitle>Create Classroom</DialogTitle>
        <DialogContent>
          <TextField autoFocus fullWidth label="Name" value={name} onChange={(e) => setName(e.target.value)} sx={{ mt: 1, mb: 2 }} />
          <TextField fullWidth label="Description" multiline rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
        </DialogContent>
        <DialogActions><Button onClick={() => setCreateDialog(false)}>Cancel</Button><Button variant="contained" onClick={handleCreate}>Create</Button></DialogActions>
      </Dialog>
    </Box>
  );
};

export default ClassroomsPage;
