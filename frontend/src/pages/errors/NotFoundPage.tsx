import { Box, Typography, Button } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

const NotFoundPage = () => (
  <Box sx={{ textAlign: 'center', py: 12 }}>
    <Typography variant="h1" sx={{ fontSize: 120, fontWeight: 700, color: 'primary.main' }}>404</Typography>
    <Typography variant="h5" gutterBottom>Page Not Found</Typography>
    <Typography color="text.secondary" sx={{ mb: 4 }}>The page you're looking for doesn't exist.</Typography>
    <Button variant="contained" component={RouterLink} to="/">Go Home</Button>
  </Box>
);

export default NotFoundPage;
