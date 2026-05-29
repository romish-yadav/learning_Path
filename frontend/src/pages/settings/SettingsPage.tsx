import { Box, Typography, Card, CardContent, Switch, FormControlLabel, Divider } from '@mui/material';

const SettingsPage = () => (
  <Box sx={{ maxWidth: 600 }}>
    <Typography variant="h4" gutterBottom>Settings</Typography>
    <Card>
      <CardContent>
        <Typography variant="h6" gutterBottom>Notifications</Typography>
        <FormControlLabel control={<Switch defaultChecked />} label="Email notifications" />
        <FormControlLabel control={<Switch defaultChecked />} label="Assignment reminders" />
        <FormControlLabel control={<Switch />} label="Community updates" />
        <Divider sx={{ my: 2 }} />
        <Typography variant="h6" gutterBottom>Privacy</Typography>
        <FormControlLabel control={<Switch defaultChecked />} label="Show profile publicly" />
        <FormControlLabel control={<Switch defaultChecked />} label="Show learning progress" />
        <Divider sx={{ my: 2 }} />
        <Typography variant="h6" gutterBottom>Appearance</Typography>
        <FormControlLabel control={<Switch />} label="Dark mode" />
      </CardContent>
    </Card>
  </Box>
);

export default SettingsPage;
