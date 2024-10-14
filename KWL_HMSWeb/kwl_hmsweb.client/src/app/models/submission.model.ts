export interface Submission {
  submission_id: number; // Unique identifier for the submission
  assignment_id: number; // Foreign key to the assignment
  user_id: number; // Foreign key to the user who made the submission
  submission_date: Date; // Date of submission
  submission_description: string; // Description of the submission
  video_path: string; // Path to the video file submitted
}
