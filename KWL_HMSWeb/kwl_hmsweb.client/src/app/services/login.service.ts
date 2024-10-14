import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = `${environment.apiUrl}/api/login`;

  constructor(private http: HttpClient) { }

  isLoggedIn(): boolean {
    // Check if token exists in localStorage (or sessionStorage/cookie)
    return !!localStorage.getItem('authToken'); // Adjust this to your login logic
  }

  // Authenticate method with separate username and password
  authenticate(username: string, password: string): Observable<any> {
    const credentials = { username, password }; // Create an object for credentials
    return this.http.post(`${this.apiUrl}/authenticate`, credentials).pipe(
      tap((response: any) => {
        if (response && response.token) {
          // Store the JWT token in localStorage or sessionStorage
          localStorage.setItem('authToken', response.token);
        }
      })
    );
  }

  // Helper function to get token from local storage
  private getAuthToken(): string | null {
    return localStorage.getItem('authToken');
  }

  // Get all logins, including JWT token in Authorization header
  getAllLogins(): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.get(`${this.apiUrl}/all`, { headers });
  }

  // View login by ID with JWT token in Authorization header
  viewLogin(id: number): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.get(`${this.apiUrl}/view/${id}`, { headers });
  }

  // Update login by ID with JWT token in Authorization header
  updateLogin(id: number, data: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.put(`${this.apiUrl}/update/${id}`, data, { headers });
  }

  // Delete login by ID with JWT token in Authorization header
  deleteLogin(id: number): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.delete(`${this.apiUrl}/delete/${id}`, { headers });
  }

  logout() {
    localStorage.removeItem('token');
  }
}




