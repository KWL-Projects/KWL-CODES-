export interface Assignment {
  assignment_id: number; // Unique identifier for the assignment
  subject_id: number; // Foreign key to the subject table
  //user_id: string; // Foreign key to the user who created the assignment
  assignment_name: string; // Name of the assignment
  assignment_description: string; // Description of the assignment
  due_date: Date; // Due date for the assignment
}
