export interface Feedback {
  feedback_id: number; // Unique identifier for the feedback
  submission_id: number; // Foreign key to the submission
  //user_id: number; // Foreign key to the user who provided the feedback
  feedback: string; // Text feedback provided
  mark_received: number; // Mark received for the submission
}
