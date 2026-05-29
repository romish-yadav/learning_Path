import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { TextField, Button, Box, Typography, Alert, Link } from '@mui/material';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { login, clearError } from '../../redux/slices/authSlice';
import { loginSchema } from '../../validations/authValidation';
import type { AppDispatch, RootState } from '../../redux/store';
import type { LoginRequest } from '../../types/auth.types';
import { useEffect } from 'react';

const LoginPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { loading, error, isAuthenticated } = useSelector((state: RootState) => state.auth);

  const { register: formRegister, handleSubmit, formState: { errors } } = useForm<LoginRequest>({
    resolver: yupResolver(loginSchema),
  });

  useEffect(() => {
    if (isAuthenticated) navigate('/dashboard');
    return () => { dispatch(clearError()); };
  }, [isAuthenticated, navigate, dispatch]);

  const onSubmit = (data: LoginRequest) => {
    dispatch(login(data));
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 2 }}>
      <Typography variant="h5" align="center" gutterBottom>Sign In</Typography>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TextField fullWidth label="Email" margin="normal" {...formRegister('email')} error={!!errors.email} helperText={errors.email?.message} />
      <TextField fullWidth label="Password" type="password" margin="normal" {...formRegister('password')} error={!!errors.password} helperText={errors.password?.message} />
      <Button fullWidth variant="contained" type="submit" disabled={loading} sx={{ mt: 2, mb: 2 }}>
        {loading ? 'Signing in...' : 'Sign In'}
      </Button>
      <Typography align="center" variant="body2">
        Don't have an account? <Link component={RouterLink} to="/register">Sign Up</Link>
      </Typography>
    </Box>
  );
};

export default LoginPage;
