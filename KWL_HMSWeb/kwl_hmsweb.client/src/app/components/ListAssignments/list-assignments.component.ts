import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AssignmentService } from '../../services/assignment.service'; // Adjust the path as necessary
import { Assignment } from '../../models/assignment.model'; // Adjust the path as necessary

@Component({
  selector: 'app-list-assignments',
  templateUrl: './list-assignments.component.html',
  styleUrls: ['./list-assignments.component.css'] // Add your styles here
})
export class ListAssignments implements OnInit {
  assignments: Assignment[] = []; // Array to hold assignments
  errorMessage: string | null = null;

  constructor(private assignmentService: AssignmentService, private router: Router) { }

  ngOnInit(): void {
    this.loadAssignments();
  }

  loadAssignments(): void {
    this.assignmentService.getAllAssignments().subscribe(
      (data: Assignment[]) => {
        this.assignments = data;
      },
      (error) => {
        this.errorMessage = 'Error loading assignments: ' + error.message;
      }
    );
  }

  viewAssignment(assignment_id: number): void {
    // Navigate to the assignment details page using the assignment ID
    this.router.navigate(['/assignment-details', assignment_id]);
  }

  deleteAssignment(assignment_id: number): void {
    this.assignmentService.deleteAssignment(assignment_id).subscribe(
      () => {
        this.loadAssignments(); // Reload assignments after deletion
      },
      (error) => {
        this.errorMessage = 'Error deleting assignment: ' + error.message;
      }
    );
  }
}



