import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http'; // Import HttpClient

@Component({
  selector: 'app-list-assignment-videos',
  templateUrl: './list-assignment-videos.component.html',
  styleUrls: ['./list-assignment-videos.component.css']
})
export class ListAssignmentVideosComponent implements OnInit {
  videos = [
    {
      id: 1,
      title: 'Video 1',
      submissionId: 1,
      assignmentId: 101,
      submissionDateTime: '2024-09-20T10:00:00',
      submissionDescription: 'First submission description'
    },
    {
      id: 2,
      title: 'Video 2',
      submissionId: 2,
      assignmentId: 102,
      submissionDateTime: '2024-09-21T12:00:00',
      submissionDescription: 'Second submission description'
    }
  ];

  constructor(private router: Router, private location: Location) { }

  ngOnInit(): void {
    // Fetch videos from the backend
  }

  openFeedback(): void {
    this.router.navigate(['/provide-feedback']);
  }

  goBack(): void {
    this.location.back();
  }
}
