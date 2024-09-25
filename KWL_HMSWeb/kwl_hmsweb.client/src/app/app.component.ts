import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isMainPage: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.checkRoute();
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkRoute();
      }
    });
  }

  checkRoute(): void {
    this.isMainPage = this.router.url === '/';
    console.log('Current URL:', this.router.url); // Debugging line
    console.log('isMainPage:', this.isMainPage); // Debugging line
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }
}
