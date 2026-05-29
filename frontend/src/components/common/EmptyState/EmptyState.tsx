import { Box, Typography, Button } from '@mui/material';
import type { ReactNode } from 'react';

interface EmptyStateProps {
  icon?: ReactNode;
  title: string;
  description?: string;
  actionLabel?: string;
  onAction?: () => void;
}

const EmptyState = ({ icon, title, description, actionLabel, onAction }: EmptyStateProps) => (
  <Box sx={{ textAlign: 'center', py: 8 }}>
    {icon && <Box sx={{ mb: 2 }}>{icon}</Box>}
    <Typography variant="h6" gutterBottom>{title}</Typography>
    {description && <Typography color="text.secondary" sx={{ mb: 2 }}>{description}</Typography>}
    {actionLabel && onAction && <Button variant="contained" onClick={onAction}>{actionLabel}</Button>}
  </Box>
);

export default EmptyState;
