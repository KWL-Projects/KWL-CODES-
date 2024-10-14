import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { LoginService } from './services/login.service';  // Correct path to your login service

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'KWL HMS Web Application';  // Make sure this property is declared
  /*showLoginMessage = false;

  // List of protected routes
  protectedRoutes: string[] = [
    '/landing',
    '/user-admin',
    '/list-assignments',
    '/create-assignment',
    '/list-assignment-videos',
    '/watch-video-feedback'
  ];

  constructor(private router: Router, private loginService: LoginService) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        // Check if the user is navigating to one of the protected routes
        if (this.protectedRoutes.includes(event.url) && !this.loginService.isLoggedIn()) {
          this.showLoginMessage = true; // Show the login message
          this.router.navigate(['/login']); // Redirect to login page
        } else {
          this.showLoginMessage = false; // Hide the message if logged in or navigating elsewhere
        }
      }
    });
  }*/
}












