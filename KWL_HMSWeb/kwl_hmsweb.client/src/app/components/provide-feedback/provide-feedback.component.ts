import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; // Import HttpClient

@Component({
  selector: 'app-provide-feedback',
  templateUrl: './provide-feedback.component.html',
  styleUrls: ['./provide-feedback.component.css']
})
export class ProvideFeedbackComponent {
  submissionId: string = '';
  userId: string = '';
  feedback: string = '';
  mark: number | null = null;

  constructor(private router: Router) { }

  onSubmit() {
    const feedbackData = {
      submissionId: this.submissionId,
      userId: this.userId,
      feedback: this.feedback,
      mark: this.mark
    };

    console.log('Feedback Data:', feedbackData);
    // Reset form after submission
    this.submissionId = '';
    this.userId = '';
    this.feedback = '';
    this.mark = null;
  }

  goToLanding(): void {
    this.router.navigate(['/landing']);
  }
}
