export interface Dashboard {
  activePaths: number;
  completedToday: number;
  streakDays: number;
  totalPoints: number;
  inProgressPaths: PathProgress[];
  recommendations: Recommendation[];
}

export interface UserAnalytics {
  totalPathsEnrolled: number;
  pathsCompleted: number;
  modulesCompleted: number;
  totalModules: number;
  overallCompletionRate: number;
  totalTimeSpentMinutes: number;
  certificatesEarned: number;
  pathProgresses: PathProgress[];
  recentActivity: Activity[];
}

export interface PathProgress {
  pathId: string;
  pathTitle: string;
  completedModules: number;
  totalModules: number;
  completionPercentage: number;
  lastActivityAt?: string;
}

export interface Activity {
  description: string;
  type: string;
  occurredAt: string;
}

export interface Recommendation {
  pathId: string;
  pathTitle: string;
  reason?: string;
  confidenceScore: number;
}

export interface ClassroomAnalytics {
  classroomId: string;
  classroomName: string;
  totalStudents: number;
  averageCompletionRate: number;
  assignmentsCreated: number;
  submissionsReceived: number;
  averageScore: number;
  studentProgresses: StudentProgress[];
}

export interface StudentProgress {
  studentId: string;
  studentName: string;
  completionRate: number;
  averageScore: number;
  modulesCompleted: number;
}
