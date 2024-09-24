// src/app/components/provide-feedback/provide-feedback.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-provide-feedback',
  templateUrl: './provide-feedback.component.html',
  styleUrls: ['./provide-feedback.component.css']
})
export class ProvideFeedbackComponent {
  feedback: string = '';

  onSubmit() {
    // Implement feedback submission logic here
    console.log('Feedback:', this.feedback);
    // Reset form after submission
    this.feedback = '';
  }
}
