import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VideoService } from '../../services/video.service'; // Adjust the path as necessary
import { FeedbackService } from '../../services/feedback.service'; // Adjust the path as necessary
import { Submission } from '../../models/submission.model'; // Adjust the path as necessary
import { Feedback } from '../../models/feedback.model'; // Adjust the path as necessary

@Component({
  selector: 'app-watch-video-feedback',
  templateUrl: './watch-video-feedback.component.html',
  styleUrls: ['./watch-video-feedback.component.css'] // Add your styles here
})
export class WatchVideoFeedback implements OnInit {
  submissionId: number;
  submission: Submission | null = null;
  feedback: Feedback | null = null;
  mark: number | null = null;
  comment: string = '';
  errorMessage: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoService,
    private feedbackService: FeedbackService,
    private router: Router
  ) {
    this.submissionId = +this.route.snapshot.paramMap.get('id')!; // Fetching submission ID from route parameters
  }

  ngOnInit(): void {
    this.loadSubmission();
    this.loadFeedback();
  }

  loadSubmission(): void {
    this.videoService.downloadSubmission(this.submissionId).subscribe(
      (data: Submission) => {
        this.submission = data;
      },
      (error) => {
        this.errorMessage = 'Error loading submission: ' + error.message;
      }
    );
  }

  loadFeedback(): void {
    this.feedbackService.getSubmissionFeedback(this.submissionId).subscribe(
      (data: Feedback) => {
        this.feedback = data;
        this.mark = data.mark_received;
        this.comment = data.feedback;
      },
      (error) => {
        this.errorMessage = 'Error loading feedback: ' + error.message;
      }
    );
  }

  submitFeedback(): void {
    const feedbackData: Feedback = {
      feedback_id: this.feedback?.feedback_id || 0,
      submission_id: this.submissionId,
      //user_id: this.submission?.user_id!, // Assuming feedback is for the user who submitted the video
      feedback: this.comment,
      mark_received: this.mark!
    };

    this.feedbackService.submitFeedback(feedbackData).subscribe(
      () => {
        alert('Feedback submitted successfully!');
        this.loadFeedback(); // Refresh feedback after submission
      },
      (error) => {
        this.errorMessage = 'Error submitting feedback: ' + error.message;
      }
    );
  }

  goBack(): void {
    this.router.navigate(['/list-assignment-videos']); // Navigate back to the assignment videos page
  }
}





