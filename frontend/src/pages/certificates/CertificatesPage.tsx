import { Box, Typography, Card, CardContent } from '@mui/material';
import { EmojiEvents } from '@mui/icons-material';

const CertificatesPage = () => (
  <Box>
    <Typography variant="h4" gutterBottom>My Certificates</Typography>
    <Card>
      <CardContent sx={{ textAlign: 'center', py: 8 }}>
        <EmojiEvents sx={{ fontSize: 64, color: 'warning.main', mb: 2 }} />
        <Typography variant="h6">No Certificates Yet</Typography>
        <Typography color="text.secondary">Complete learning paths to earn certificates.</Typography>
      </CardContent>
    </Card>
  </Box>
);

export default CertificatesPage;
