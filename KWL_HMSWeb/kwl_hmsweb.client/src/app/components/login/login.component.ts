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

  onSubmit() {
    this.http.post('/api/auth/login', { username: this.username, password: this.password })
      .subscribe(response => {
        // Handle successful login
        this.router.navigate(['/landing']);
      }, error => {
        // Handle login error
        console.error('Login failed', error);
      });
  }
}
