import * as yup from 'yup';

export const createClassroomSchema = yup.object({
  name: yup.string().max(200).required('Classroom name is required'),
  description: yup.string().max(1000),
});

export const createAssignmentSchema = yup.object({
  title: yup.string().max(300).required('Title is required'),
  description: yup.string().max(2000),
  instructions: yup.string(),
  dueDate: yup.string(),
  maxScore: yup.number().min(1).max(100).required(),
});

export const joinClassroomSchema = yup.object({
  joinCode: yup.string().required('Join code is required'),
});
