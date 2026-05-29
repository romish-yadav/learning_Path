import * as yup from 'yup';

export const createPathSchema = yup.object({
  title: yup.string().max(200).required('Title is required'),
  description: yup.string().max(2000),
  difficulty: yup.string().oneOf(['Beginner', 'Intermediate', 'Advanced', 'Expert']).required(),
  isPublic: yup.boolean().required(),
  estimatedHours: yup.number().min(0).required('Estimated hours is required'),
  tags: yup.string().max(500),
});

export const createModuleSchema = yup.object({
  title: yup.string().max(200).required('Title is required'),
  description: yup.string().max(2000),
  type: yup.string().oneOf(['Lesson', 'Quiz', 'Assignment', 'Video', 'Article', 'Project']).required(),
  orderIndex: yup.number().min(0).required(),
  estimatedMinutes: yup.number().min(0).required(),
});
