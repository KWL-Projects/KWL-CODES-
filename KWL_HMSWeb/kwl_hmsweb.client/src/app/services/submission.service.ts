import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class SubmissionService {
  private apiUrl = `${environment.apiUrl}/api/submission`;

  constructor(private http: HttpClient) { }

  createSubmission(submissionData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, submissionData);
  }

  getAllSubmissions(): Observable<any> {
    return this.http.get(`${this.apiUrl}/all`);
  }

  viewOwnSubmissions(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/viewOwn/${userId}`);
  }

  viewSubmission(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/view/${id}`);
  }

  updateSubmission(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${id}`, data);
  }

  deleteSubmission(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}


