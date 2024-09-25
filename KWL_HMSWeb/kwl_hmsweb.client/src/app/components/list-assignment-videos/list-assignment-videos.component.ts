import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-assignment-videos',
  templateUrl: './view-assignment-videos.component.html',
  styleUrls: ['./view-assignment-videos.component.css']
})
export class ListAssignmentVideosComponent implements OnInit {
  videos = [
    {
      id: 1,
      title: 'Video 1',
      submissionId: 1,
      assignmentId: 101,
      submissionDateTime: new Date('2024-09-20T10:00:00'),
      submissionDescription: 'First submission description'
    },
    {
      id: 2,
      title: 'Video 2',
      submissionId: 2,
      assignmentId: 102,
      submissionDateTime: new Date('2024-09-21T12:00:00'),
      submissionDescription: 'Second submission description'
    }
  ];
  /*submissions: any[] = [];

  constructor(private submissionService: SubmissionService, private router: Router) { }

  ngOnInit(): void {
    this.submissionService.getSubmissions().subscribe(data => {
      this.submissions = data;
    });
  }*/
  constructor(private router: Router) { }

  ngOnInit(): void {
    // Fetch videos from the backend
  }

  openFeedback(): void {
    this.router.navigate(['/provide-feedback']);
  }
}
