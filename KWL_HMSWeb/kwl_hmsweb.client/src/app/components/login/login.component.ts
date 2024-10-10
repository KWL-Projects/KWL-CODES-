import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment'; // Correct import path

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = ''; // For displaying error messages

  constructor(private http: HttpClient, private router: Router) { }

  onSubmit() {
    const loginData = { username: this.username, password: this.password };

    // Make the login request to the API
    this.http.post(`${environment.apiUrl}login/authenticate`, loginData)
      .subscribe(
        (response: any) => {
          const token = response.token;
          localStorage.setItem('jwtToken', token);
          this.router.navigate(['/user-administration']);
        },
        (error) => {
          this.errorMessage = 'Login failed: Invalid username or password';
          console.error('Login failed', error);
        }
      );
  }

  // New method to navigate to login
  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwtToken');
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }
}






