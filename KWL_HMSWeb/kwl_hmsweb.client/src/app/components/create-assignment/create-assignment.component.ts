import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common'; // Import Location service

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.css']
})
export class CreateAssignmentComponent {
  assignment = {
    subjectId: '',
    userId: '',
    name: '',
    description: '',
    dueDate: ''
  };

  constructor(private router: Router, private location: Location) { } // Inject Location service

  onSubmit() {
    // Implement create assignment logic here
    console.log('Assignment:', this.assignment);
    // Reset form after submission
    this.assignment = {
      subjectId: '',
      userId: '',
      name: '',
      description: '',
      dueDate: ''
    };
  }

  navigateToVideos() {
    this.router.navigate(['/list-assignment-videos']);
  }

  goBack() {
    // Logic to navigate back to the previous page
    this.location.back();
  }
}
