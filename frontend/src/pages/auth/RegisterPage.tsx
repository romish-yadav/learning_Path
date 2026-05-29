import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { TextField, Button, Box, Typography, Alert, Link, Grid } from '@mui/material';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { register as registerAction, clearError } from '../../redux/slices/authSlice';
import { registerSchema } from '../../validations/authValidation';
import type { AppDispatch, RootState } from '../../redux/store';
import type { RegisterRequest } from '../../types/auth.types';
import { useEffect } from 'react';

const RegisterPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { loading, error, isAuthenticated } = useSelector((state: RootState) => state.auth);

  const { register: formRegister, handleSubmit, formState: { errors } } = useForm<RegisterRequest>({
    resolver: yupResolver(registerSchema),
  });

  useEffect(() => {
    if (isAuthenticated) navigate('/dashboard');
    return () => { dispatch(clearError()); };
  }, [isAuthenticated, navigate, dispatch]);

  const onSubmit = (data: RegisterRequest) => {
    dispatch(registerAction(data));
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 2 }}>
      <Typography variant="h5" align="center" gutterBottom>Create Account</Typography>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <Grid container spacing={2}>
        <Grid size={{ xs: 6 }}>
          <TextField fullWidth label="First Name" {...formRegister('firstName')} error={!!errors.firstName} helperText={errors.firstName?.message} />
        </Grid>
        <Grid size={{ xs: 6 }}>
          <TextField fullWidth label="Last Name" {...formRegister('lastName')} error={!!errors.lastName} helperText={errors.lastName?.message} />
        </Grid>
      </Grid>
      <TextField fullWidth label="Email" margin="normal" {...formRegister('email')} error={!!errors.email} helperText={errors.email?.message} />
      <TextField fullWidth label="Password" type="password" margin="normal" {...formRegister('password')} error={!!errors.password} helperText={errors.password?.message} />
      <TextField fullWidth label="Confirm Password" type="password" margin="normal" {...formRegister('confirmPassword')} error={!!errors.confirmPassword} helperText={errors.confirmPassword?.message} />
      <Button fullWidth variant="contained" type="submit" disabled={loading} sx={{ mt: 2, mb: 2 }}>
        {loading ? 'Creating account...' : 'Sign Up'}
      </Button>
      <Typography align="center" variant="body2">
        Already have an account? <Link component={RouterLink} to="/login">Sign In</Link>
      </Typography>
    </Box>
  );
};

export default RegisterPage;
