import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AssignmentService } from '../../services/assignment.service'; // Adjust the path as necessary
import { SubjectService } from '../../services/subject.service'; // Service to fetch subjects
import { Assignment } from '../../models/assignment.model'; // Adjust the path as necessary
import { Subject } from '../../models/subject.model'; // Adjust the path as necessary

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.css'] // Add your styles here
})
export class CreateAssignment implements OnInit {
  assignment: Assignment = {
    assignment_id: 1,
    assignment_name: '',
    assignment_description: '',
    due_date: new Date(),
    subject_id: 1, // Assume subject_id is part of Assignment model
  };
  subjects: Subject[] = [];
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private assignmentService: AssignmentService, private subjectService: SubjectService, private router: Router) { }

  // Use the router for navigation
  navigateToListAssignments() {
    this.router.navigate(['/list-assignment']);
  }

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.subjectService.getAllSubjects().subscribe(
      (data: Subject[]) => {
        this.subjects = data;
      },
      (error) => {
        this.errorMessage = 'Error loading subjects: ' + error.message;
      }
    );
  }

  createAssignment(): void {
    this.assignmentService.createAssignment(this.assignment).subscribe(
      () => {
        this.successMessage = 'Assignment created successfully!';
        this.errorMessage = null;
        this.router.navigate(['/list-assignments']); // Redirect to the list assignments page
      },
      (error) => {
        this.errorMessage = 'Error creating assignment: ' + error.message;
        this.successMessage = null;
      }
    );
  }
}




