import { Outlet } from 'react-router-dom';
import { Box, Typography } from '@mui/material';

const AdminLayout = () => (
  <Box>
    <Box sx={{ mb: 3 }}>
      <Typography variant="h4" gutterBottom>Admin Panel</Typography>
    </Box>
    <Outlet />
  </Box>
);

export default AdminLayout;
