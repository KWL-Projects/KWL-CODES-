import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-assignment-videos',
  templateUrl: './list-assignment-videos.component.html',
  styleUrls: ['./list-assignment-videos.component.css']
})
export class ListAssignmentVideosComponent implements OnInit {
  videos = [
    { id: 1, title: 'Video 1' },
    { id: 2, title: 'Video 2' }
  ];

  constructor(private router: Router) { }

  ngOnInit(): void {
    // Fetch videos from the backend
  }

  openFeedback(): void {
    this.router.navigate(['/provide-feedback']);
  }
}
