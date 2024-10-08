import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private http: HttpClient, private router: Router) { }

  navigateToLogin() {
    this.router.navigate(['/landing']);
  }

  /*onSubmit() {
    this.http.post('/api/auth/login', { username: this.username, password: this.password })
      .subscribe(response => {
        // Handle successful login
        this.router.navigate(['/landing']);
      }, error => {
        // Handle login error
        console.error('Login failed', error);
      });*/

  /*onSubmit() {
    this.http.post('https://localhost:5001/api/login/authenticate',
      { username: this.username, password: this.password })
      .subscribe(response => {
        // Handle successful login, e.g., save the token to localStorage
        this.router.navigate(['/landing']);
      }, error => {
        // Handle login error
        console.error('Login failed', error);
      });*/
  onSubmit() {
    this.http.post('https://localhost:5001/api/login/authenticate',
      { username: this.username, password: this.password })
      .subscribe((response: any) => {
        // Assuming the token is in the response
        const token = response.token;
        localStorage.setItem('jwtToken', token);
        this.router.navigate(['/landing']);
      }, error => {
        console.error('Login failed', error);
      });
  }


  }

