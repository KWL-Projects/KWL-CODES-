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
  videos: any[] = [];
  private apiUrl = 'https://localhost:7074/api/submission/all'; // API endpoint

  constructor(private router: Router, private location: Location, private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any[]>(this.apiUrl).subscribe(data => {
      this.videos = data;
    }, error => {
      console.error('Error fetching submissions:', error);
    });
  }

  openFeedback(): void {
    this.router.navigate(['/provide-feedback']);
  }

  goBack(): void {
    this.location.back();
  }
}
