import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class AssignmentService {
  private apiUrl = `${environment.apiUrl}/api/assignment`;

  constructor(private http: HttpClient) { }

  createAssignment(assignmentData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, assignmentData);
  }

  getAllAssignments(): Observable<any> {
    return this.http.get(`${this.apiUrl}/all`);
  }

  viewAssignment(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/view/${id}`);
  }

  updateAssignment(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${id}`, data);
  }

  deleteAssignment(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}

