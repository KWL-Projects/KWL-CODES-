// src/app/components/list-assignments/list-assignments.component.ts
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-assignments',
  templateUrl: './list-assignments.component.html',
  styleUrls: ['./list-assignments.component.css']
})
export class ListAssignmentsComponent implements OnInit {
  assignments = [
    { id: 1, title: 'Assignment 1' },
    { id: 2, title: 'Assignment 2' }
  ];

  constructor() { }

  ngOnInit(): void {
    // Fetch assignments from the backend
  }
}
