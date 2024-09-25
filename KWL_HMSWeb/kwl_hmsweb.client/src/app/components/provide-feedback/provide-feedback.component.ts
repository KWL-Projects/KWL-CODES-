import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-provide-feedback',
  templateUrl: './provide-feedback.component.html',
  styleUrls: ['./provide-feedback.component.css']
})
export class ProvideFeedbackComponent {
  feedback: string = '';

  constructor(private router: Router) { }

  onSubmit() {
    // Implement feedback submission logic here
    console.log('Feedback:', this.feedback);
    // Reset form after submission
    this.feedback = '';
  }

  goToLanding(): void {
    this.router.navigate(['/landing']);
  }
}
