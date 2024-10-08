import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; // Import HttpClient

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent {
  constructor(private router: Router) { }

  navigateToUserAdmin() {
    this.router.navigate(['/user-administration']);
  }

  navigateToListAssignments() {
    this.router.navigate(['/list-assignments']);
  }

  navigateToCreateAssignment() {
    this.router.navigate(['/create-assignment']);
  }
}
