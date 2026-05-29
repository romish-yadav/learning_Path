import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { TextField, Button, Box, Typography, Select, MenuItem, FormControl, InputLabel, Switch, FormControlLabel, Alert } from '@mui/material';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createPathSchema } from '../../validations/pathValidation';
import pathService from '../../services/pathService';
import { DIFFICULTY_OPTIONS } from '../../constants';
import type { CreateLearningPathRequest, PathDifficulty } from '../../types/path.types';

interface FormData {
  title: string;
  description?: string;
  difficulty: string;
  isPublic: boolean;
  estimatedHours: number;
  tags?: string;
}

const CreatePathPage = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const { register, handleSubmit, formState: { errors }, setValue, watch } = useForm<FormData>({
    resolver: yupResolver(createPathSchema) as any,
    defaultValues: { difficulty: 'Beginner', isPublic: true, estimatedHours: 1 },
  });

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    try {
      const request: CreateLearningPathRequest = {
        title: data.title,
        description: data.description,
        difficulty: data.difficulty as PathDifficulty,
        isPublic: data.isPublic,
        estimatedHours: data.estimatedHours,
        tags: data.tags,
      };
      const response = await pathService.create(request);
      if (response.data.data) navigate(`/paths/${response.data.data.id}`);
    } catch {
      setError('Failed to create learning path');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ maxWidth: 600 }}>
      <Typography variant="h4" gutterBottom>Create Learning Path</Typography>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TextField fullWidth label="Title" margin="normal" {...register('title')} error={!!errors.title} helperText={errors.title?.message} />
      <TextField fullWidth label="Description" margin="normal" multiline rows={4} {...register('description')} error={!!errors.description} helperText={errors.description?.message} />
      <FormControl fullWidth margin="normal">
        <InputLabel>Difficulty</InputLabel>
        <Select value={watch('difficulty')} onChange={(e) => setValue('difficulty', e.target.value)} label="Difficulty">
          {DIFFICULTY_OPTIONS.map((d) => <MenuItem key={d} value={d}>{d}</MenuItem>)}
        </Select>
      </FormControl>
      <TextField fullWidth label="Estimated Hours" type="number" margin="normal" {...register('estimatedHours')} error={!!errors.estimatedHours} />
      <TextField fullWidth label="Tags (comma separated)" margin="normal" {...register('tags')} />
      <FormControlLabel control={<Switch checked={watch('isPublic')} onChange={(e) => setValue('isPublic', e.target.checked)} />} label="Public" sx={{ mt: 1 }} />
      <Box sx={{ mt: 3 }}>
        <Button variant="contained" type="submit" disabled={loading}>{loading ? 'Creating...' : 'Create Path'}</Button>
      </Box>
    </Box>
  );
};

export default CreatePathPage;
