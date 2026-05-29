import { Box, CircularProgress, Typography } from '@mui/material';

interface LoaderProps {
  message?: string;
}

const Loader = ({ message = 'Loading...' }: LoaderProps) => (
  <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '200px', gap: 2 }}>
    <CircularProgress />
    <Typography variant="body2" color="text.secondary">{message}</Typography>
  </Box>
);

export default Loader;
