export type PathDifficulty = 'Beginner' | 'Intermediate' | 'Advanced' | 'Expert';
export type ModuleType = 'Lesson' | 'Quiz' | 'Assignment' | 'Video' | 'Article' | 'Project';

export interface LearningPathSummary {
  id: string;
  title: string;
  description?: string;
  thumbnailUrl?: string;
  difficulty: PathDifficulty;
  isPublished: boolean;
  estimatedHours: number;
  tags?: string;
  moduleCount: number;
  averageRating: number;
  creatorName: string;
  createdAt: string;
}

export interface LearningPathDetail extends LearningPathSummary {
  creatorId: string;
  isPublic: boolean;
  modules: Module[];
  ratingCount: number;
}

export interface Module {
  id: string;
  title: string;
  description?: string;
  content?: string;
  resourceUrl?: string;
  type: ModuleType;
  orderIndex: number;
  estimatedMinutes: number;
  prerequisiteIds: string[];
}

export interface CreateLearningPathRequest {
  title: string;
  description?: string;
  thumbnailUrl?: string;
  difficulty: PathDifficulty;
  isPublic: boolean;
  tags?: string;
  estimatedHours: number;
}

export interface UpdateLearningPathRequest {
  title?: string;
  description?: string;
  thumbnailUrl?: string;
  difficulty?: PathDifficulty;
  isPublic?: boolean;
  isPublished?: boolean;
  tags?: string;
  estimatedHours?: number;
}

export interface CreateModuleRequest {
  title: string;
  description?: string;
  content?: string;
  resourceUrl?: string;
  type: ModuleType;
  orderIndex: number;
  estimatedMinutes: number;
  prerequisiteIds?: string[];
}

export interface UpdateModuleRequest {
  title?: string;
  description?: string;
  content?: string;
  resourceUrl?: string;
  type?: ModuleType;
  orderIndex?: number;
  estimatedMinutes?: number;
  prerequisiteIds?: string[];
}
