export interface ClassroomSummary {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
  studentCount: number;
  pathCount: number;
  instructorName: string;
  createdAt: string;
}

export interface ClassroomDetail extends ClassroomSummary {
  joinCode?: string;
  instructorId: string;
  members: ClassroomMember[];
  assignments: AssignmentSummary[];
}

export interface ClassroomMember {
  userId: string;
  name: string;
  email: string;
  avatarUrl?: string;
  role: string;
  joinedAt: string;
}

export interface CreateClassroomRequest {
  name: string;
  description?: string;
}

export interface UpdateClassroomRequest {
  name?: string;
  description?: string;
  isActive?: boolean;
}

export interface AssignmentSummary {
  id: string;
  title: string;
  description?: string;
  dueDate?: string;
  maxScore: number;
  submissionCount: number;
  createdAt: string;
}

export interface AssignmentDetail extends AssignmentSummary {
  instructions?: string;
  moduleId?: string;
  classroomId: string;
  submissions: Submission[];
}

export interface Submission {
  id: string;
  content?: string;
  fileUrl?: string;
  score?: number;
  feedback?: string;
  status: string;
  studentName: string;
  submittedAt: string;
  gradedAt?: string;
}

export interface CreateAssignmentRequest {
  title: string;
  description?: string;
  instructions?: string;
  dueDate?: string;
  maxScore: number;
  moduleId?: string;
}

export interface CreateSubmissionRequest {
  content?: string;
  fileUrl?: string;
}

export interface GradeSubmissionRequest {
  score: number;
  feedback?: string;
}
