// src/app/components/create-assignment/create-assignment.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.css']
})
export class CreateAssignmentComponent {
  assignment = { title: '', description: '' };

  onSubmit() {
    // Implement create assignment logic here
    console.log('Assignment:', this.assignment);
    // Reset form after submission
    this.assignment = { title: '', description: '' };
  }
}
