import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http'; // Import HttpClient

@Component({
  selector: 'app-list-assignments',
  templateUrl: './list-assignments.component.html',
  styleUrls: ['./list-assignments.component.css']
})
export class ListAssignmentsComponent implements OnInit {
  assignments: any[] = [];
  private apiUrl = 'https://localhost:7074/api/assignment/all'; // API endpoint

  constructor(private location: Location, private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any[]>(this.apiUrl).subscribe(data => {
      this.assignments = data;
    }, error => {
      console.error('Error fetching assignments:', error);
    });
  }

  goBack(): void {
    this.location.back();
  }
}
