import { Box, Typography, List, ListItem, ListItemText, Chip, Card, Button } from '@mui/material';
import { useNotifications } from '../../hooks/useNotifications';
import { timeAgo } from '../../utils/dateUtils';

const NotificationsPage = () => {
  const { notifications } = useNotifications();

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Notifications</Typography>
        <Button variant="outlined">Mark all as read</Button>
      </Box>
      <Card>
        {notifications.length > 0 ? (
          <List>
            {notifications.map((n) => (
              <ListItem key={n.id} sx={{ bgcolor: n.isRead ? 'transparent' : 'action.hover' }}>
                <ListItemText
                  primary={n.title}
                  secondary={
                    <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
                      <Typography variant="caption">{n.message}</Typography>
                      <Chip label={n.type} size="small" variant="outlined" />
                      <Typography variant="caption" color="text.secondary">{timeAgo(n.createdAt)}</Typography>
                    </Box>
                  }
                />
              </ListItem>
            ))}
          </List>
        ) : (
          <Box sx={{ textAlign: 'center', py: 8 }}>
            <Typography color="text.secondary">No notifications yet.</Typography>
          </Box>
        )}
      </Card>
    </Box>
  );
};

export default NotificationsPage;
