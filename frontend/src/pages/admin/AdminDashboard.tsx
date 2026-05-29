import { useEffect, useState } from 'react';
import { Grid, Card, CardContent, Typography, Box } from '@mui/material';
import { People, School, Class, MenuBook } from '@mui/icons-material';
import adminService from '../../services/adminService';

const AdminDashboard = () => {
  const [stats, setStats] = useState({ totalUsers: 0, totalPaths: 0, totalClassrooms: 0, totalModules: 0, publishedPaths: 0 });

  useEffect(() => {
    adminService.getStats().then((res) => {
      if (res.data.data) setStats(res.data.data);
    });
  }, []);

  const cards = [
    { icon: <People sx={{ fontSize: 40 }} />, label: 'Total Users', value: stats.totalUsers, color: '#1976d2' },
    { icon: <School sx={{ fontSize: 40 }} />, label: 'Learning Paths', value: stats.totalPaths, color: '#9c27b0' },
    { icon: <Class sx={{ fontSize: 40 }} />, label: 'Classrooms', value: stats.totalClassrooms, color: '#2e7d32' },
    { icon: <MenuBook sx={{ fontSize: 40 }} />, label: 'Modules', value: stats.totalModules, color: '#ed6c02' },
  ];

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Admin Dashboard</Typography>
      <Grid container spacing={3}>
        {cards.map((card) => (
          <Grid size={{ xs: 12, sm: 6, md: 3 }} key={card.label}>
            <Card>
              <CardContent sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                <Box sx={{ color: card.color }}>{card.icon}</Box>
                <Box>
                  <Typography variant="h4" sx={{ fontWeight: 700 }}>{card.value}</Typography>
                  <Typography variant="body2" color="text.secondary">{card.label}</Typography>
                </Box>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default AdminDashboard;
