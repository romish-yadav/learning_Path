import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { TextField, Button, Box, Typography, Avatar, Alert } from '@mui/material';
import { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import authService from '../../services/authService';
import { updateProfileSchema } from '../../validations/profileValidation';

interface FormData {
  firstName?: string;
  lastName?: string;
  bio?: string;
  avatarUrl?: string;
}

const ProfilePage = () => {
  const { user } = useAuth();
  const [success, setSuccess] = useState('');
  const [error, setError] = useState('');

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: yupResolver(updateProfileSchema) as any,
    defaultValues: {
      firstName: user?.firstName,
      lastName: user?.lastName,
      bio: user?.bio ?? '',
      avatarUrl: user?.avatarUrl ?? '',
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      await authService.updateProfile(data);
      setSuccess('Profile updated successfully');
      setError('');
    } catch {
      setError('Failed to update profile');
    }
  };

  return (
    <Box sx={{ maxWidth: 600 }}>
      <Typography variant="h4" gutterBottom>Profile</Typography>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 3 }}>
        <Avatar sx={{ width: 80, height: 80, fontSize: 32 }}>{user?.firstName?.[0]}</Avatar>
        <Box>
          <Typography variant="h6">{user?.firstName} {user?.lastName}</Typography>
          <Typography variant="body2" color="text.secondary">{user?.email}</Typography>
        </Box>
      </Box>
      {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <Box component="form" onSubmit={handleSubmit(onSubmit)}>
        <TextField fullWidth label="First Name" margin="normal" {...register('firstName')} error={!!errors.firstName} helperText={errors.firstName?.message} />
        <TextField fullWidth label="Last Name" margin="normal" {...register('lastName')} error={!!errors.lastName} helperText={errors.lastName?.message} />
        <TextField fullWidth label="Bio" margin="normal" multiline rows={3} {...register('bio')} error={!!errors.bio} helperText={errors.bio?.message} />
        <TextField fullWidth label="Avatar URL" margin="normal" {...register('avatarUrl')} error={!!errors.avatarUrl} helperText={errors.avatarUrl?.message} />
        <Button variant="contained" type="submit" sx={{ mt: 2 }}>Update Profile</Button>
      </Box>
    </Box>
  );
};

export default ProfilePage;
