import { Box, Typography, Card, CardContent } from '@mui/material';
import { Forum } from '@mui/icons-material';

const CommunityPage = () => (
  <Box>
    <Typography variant="h4" gutterBottom>Community</Typography>
    <Card>
      <CardContent sx={{ textAlign: 'center', py: 8 }}>
        <Forum sx={{ fontSize: 64, color: 'primary.main', mb: 2 }} />
        <Typography variant="h6">Community Discussions</Typography>
        <Typography color="text.secondary">Join discussions on learning paths, share tips, and connect with fellow learners.</Typography>
      </CardContent>
    </Card>
  </Box>
);

export default CommunityPage;
