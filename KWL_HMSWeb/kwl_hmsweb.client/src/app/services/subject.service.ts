import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private apiUrl = `${environment.apiUrl}/api/subject`;

  constructor(private http: HttpClient) { }

  createSubject(subjectData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, subjectData);
  }

  getAllSubjects(): Observable<any> {
    return this.http.get(`${this.apiUrl}/all`);
  }

  viewSubject(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/view/${id}`);
  }

  updateSubject(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${id}`, data);
  }

  deleteSubject(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}

