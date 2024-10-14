import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login.service'; // Adjust the path as necessary
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'] // Add your styles here
})
export class LoginPage {
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private router: Router,
    private loginService: LoginService,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;

      this.loginService.authenticate(username, password).subscribe({
        next: (response) => {
          // Assuming the response contains a token or success indication
          if (response.success) {
            this.router.navigate(['/landing']); // Navigate to the landing page on success
          } else {
            this.errorMessage = response.message; // Display error message
          }
        },
        error: (err) => {
          this.errorMessage = 'Login failed. Please try again.'; // Handle login error
        }
      });
    }
  }
}






