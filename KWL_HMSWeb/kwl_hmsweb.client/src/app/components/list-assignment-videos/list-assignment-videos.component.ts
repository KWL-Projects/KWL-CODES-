// src/app/components/list-assignment-videos/list-assignment-videos.component.ts
import { Component, OnInit } from '@angular/core';

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

  constructor() { }

  ngOnInit(): void {
    // Fetch videos from the backend
  }
}
