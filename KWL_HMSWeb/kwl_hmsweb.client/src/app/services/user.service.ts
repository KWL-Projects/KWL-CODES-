import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
//import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `https://localhost:7074/api/user`;

  constructor(private http: HttpClient) { }

  // Register user with JWT token handling
  registerUser(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData).pipe(
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

  // Get all users with JWT token in Authorization header
  /*getAllUsers(): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.get(`${this.apiUrl}/all`, { headers });
  }*/

  getAllUsers(): Observable<any[]> {
    // No token is being used, so we can directly make the request
    return this.http.get<any[]>(`${this.apiUrl}/all`).pipe(
      tap((data) => console.log('Fetched users:', data)), // Log fetched users
      catchError((error) => {
        console.error('Error fetching users:', error); // Log error details
        return throwError(() => new Error('Error fetching users')); // Return the correct error object
      })
    );
  }


  // View user by ID with JWT token in Authorization header
  viewUser(id: number): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.get(`${this.apiUrl}/view/${id}`, { headers });
  }

  // Update user by ID with JWT token in Authorization header
  updateUser(id: number, data: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.put(`${this.apiUrl}/update/${id}`, data, { headers });
  }

  // Delete user by ID with JWT token in Authorization header
  deleteUser(id: number): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAuthToken()}`);
    return this.http.delete(`${this.apiUrl}/delete/${id}`, { headers });
  }
}



