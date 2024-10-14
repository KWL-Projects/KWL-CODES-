export interface Subject {
  subject_id: number; // Unique identifier for the subject
  user_id: number; // Foreign key to the user (lecturer) associated with the subject
  subject_name: string; // Name of the subject
  subject_description: string; // Description of the subject
}
