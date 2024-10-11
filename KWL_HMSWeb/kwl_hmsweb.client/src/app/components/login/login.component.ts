import { Component } from '@angular/core';
import { AuthService } from '../../auth.service'; // Adjust the import path if necessary
import { Router } from '@angular/router'; // Import Router

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string | null = null; // Store error message if login fails

  constructor(private authService: AuthService, private router: Router) { } // Inject Router

  onSubmit() {
    // Reset the error message on new submission
    this.errorMessage = null;

    // Call the AuthService login method
    this.authService.login(this.username, this.password).subscribe(
      (response) => {
        // Handle successful login response
        console.log('Login successful', response);

        // Navigate to the dashboard or home page
        this.router.navigate(['/dashboard']); // Adjust the path as needed
      },
      (error) => {
        // Handle login failure
        console.error('Login failed', error);
        this.errorMessage = 'Incorrect username or password'; // Display error message
      }
    );
  }
}











