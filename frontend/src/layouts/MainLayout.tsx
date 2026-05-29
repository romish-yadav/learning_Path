import { Outlet } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container, Box } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

const MainLayout = () => (
  <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component={RouterLink} to="/" sx={{ flexGrow: 1, textDecoration: 'none', color: 'inherit' }}>
          LearnPath
        </Typography>
        <Button color="inherit" component={RouterLink} to="/paths">Explore</Button>
        <Button color="inherit" component={RouterLink} to="/login">Login</Button>
        <Button color="inherit" variant="outlined" component={RouterLink} to="/register" sx={{ ml: 1 }}>
          Sign Up
        </Button>
      </Toolbar>
    </AppBar>
    <Container maxWidth="lg" sx={{ flex: 1, py: 4 }}>
      <Outlet />
    </Container>
    <Box component="footer" sx={{ py: 3, textAlign: 'center', bgcolor: 'grey.100' }}>
      <Typography variant="body2" color="text.secondary">
        &copy; {new Date().getFullYear()} LearnPath. All rights reserved.
      </Typography>
    </Box>
  </Box>
);

export default MainLayout;
