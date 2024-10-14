import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'] // Add your styles here
})
export class HomePage {
  constructor(private router: Router) { }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }
}



