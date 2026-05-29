export interface Comment {
  id: string;
  content: string;
  authorId: string;
  authorName: string;
  authorAvatarUrl?: string;
  createdAt: string;
  replies: Comment[];
}

export interface CreateCommentRequest {
  content: string;
  targetType: string;
  targetId: string;
  parentCommentId?: string;
}

export interface Rating {
  id: string;
  score: number;
  review?: string;
  userName: string;
  createdAt: string;
}

export interface CreateRatingRequest {
  score: number;
  review?: string;
}

export interface Notification {
  id: string;
  title: string;
  message?: string;
  type: string;
  isRead: boolean;
  actionUrl?: string;
  createdAt: string;
}
