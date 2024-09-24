import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.css']
})
export class CreateAssignmentComponent {
  assignment = { title: '', description: '' };

  constructor(private router: Router) { }

  onSubmit() {
    // Implement create assignment logic here
    console.log('Assignment:', this.assignment);
    // Reset form after submission
    this.assignment = { title: '', description: '' };
  }

  navigateToVideos() {
    this.router.navigate(['/list-assignment-videos']);
  }
}
