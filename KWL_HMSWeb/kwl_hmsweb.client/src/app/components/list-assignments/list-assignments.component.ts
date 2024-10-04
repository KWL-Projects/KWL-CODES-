import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-list-assignments',
  templateUrl: './list-assignments.component.html',
  styleUrls: ['./list-assignments.component.css']
})
export class ListAssignmentsComponent implements OnInit {
  assignments = [
    {
      id: 1,
      subjectId: 'Math101',
      userId: 'user123',
      name: 'Assignment 1',
      description: 'Description for Assignment 1',
      dueDate: new Date('2024-10-01T10:00:00')
    },
    {
      id: 2,
      subjectId: 'Sci102',
      userId: 'user456',
      name: 'Assignment 2',
      description: 'Description for Assignment 2',
      dueDate: new Date('2024-10-15T12:00:00')
    }
  ];

  constructor(private location: Location) { }

  ngOnInit(): void {
    // Fetch assignments from the backend
  }
  /*export class ListAssignmentsComponent implements OnInit {
  assignments: any[] = [];

  constructor(private assignmentService: AssignmentService) { }

  ngOnInit(): void {
    this.assignmentService.getAssignments().subscribe(data => {
      this.assignments = data;
    });
  }
}*/


  goBack(): void {
    this.location.back();
  }
}

