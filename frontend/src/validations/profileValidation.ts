import * as yup from 'yup';

export const updateProfileSchema = yup.object({
  firstName: yup.string().max(100),
  lastName: yup.string().max(100),
  bio: yup.string().max(1000),
  avatarUrl: yup.string().url('Must be a valid URL').max(500),
});
