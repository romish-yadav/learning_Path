import { Box, Typography, Button, Grid, Card, CardContent, Container } from '@mui/material';
import { School, Analytics, People, EmojiEvents } from '@mui/icons-material';
import { Link as RouterLink } from 'react-router-dom';

const features = [
  { icon: <School sx={{ fontSize: 48, color: 'primary.main' }} />, title: 'Graph-Based Learning', description: 'Navigate learning paths structured as directed acyclic graphs with clear prerequisites.' },
  { icon: <Analytics sx={{ fontSize: 48, color: 'secondary.main' }} />, title: 'Progress Analytics', description: 'Track your learning journey with detailed analytics and insights.' },
  { icon: <People sx={{ fontSize: 48, color: 'success.main' }} />, title: 'Collaborative Classrooms', description: 'Join classrooms, complete assignments, and learn with peers.' },
  { icon: <EmojiEvents sx={{ fontSize: 48, color: 'warning.main' }} />, title: 'Certificates', description: 'Earn certificates upon completing learning paths.' },
];

const HomePage = () => (
  <Box>
    <Box sx={{ textAlign: 'center', py: 8 }}>
      <Typography variant="h2" gutterBottom sx={{ fontWeight: 700 }}>
        Master Any Skill with <Box component="span" sx={{ color: 'primary.main' }}>LearnPath</Box>
      </Typography>
      <Typography variant="h5" color="text.secondary" sx={{ mb: 4, maxWidth: 600, mx: 'auto' }}>
        A graph-based learning platform that helps you navigate structured learning paths with AI-powered recommendations.
      </Typography>
      <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center' }}>
        <Button variant="contained" size="large" component={RouterLink} to="/register">Get Started</Button>
        <Button variant="outlined" size="large" component={RouterLink} to="/paths">Explore Paths</Button>
      </Box>
    </Box>

    <Container maxWidth="lg">
      <Grid container spacing={4} sx={{ mt: 4 }}>
        {features.map((feature) => (
          <Grid size={{ xs: 12, sm: 6, md: 3 }} key={feature.title}>
            <Card sx={{ height: '100%', textAlign: 'center', p: 2 }}>
              <CardContent>
                {feature.icon}
                <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>{feature.title}</Typography>
                <Typography variant="body2" color="text.secondary">{feature.description}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  </Box>
);

export default HomePage;
