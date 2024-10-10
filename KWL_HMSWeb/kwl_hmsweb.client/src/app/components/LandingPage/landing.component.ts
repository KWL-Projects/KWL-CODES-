import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'] // Add your styles here
})
export class LandingPage {
  constructor(private router: Router) { }

  navigateToUserAdministration(): void {
    this.router.navigate(['/user-admin']);
  }

  navigateToListAssignments(): void {
    this.router.navigate(['/list-assignments']);
  }

  navigateToCreateAssignment(): void {
    this.router.navigate(['/create-assignment']);
  }
}



