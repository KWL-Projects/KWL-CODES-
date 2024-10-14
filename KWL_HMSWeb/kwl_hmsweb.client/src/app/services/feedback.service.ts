import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  private apiUrl = `${environment.apiUrl}/api/feedback`;

  constructor(private http: HttpClient) { }

  submitFeedback(feedbackData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/submit`, feedbackData);
  }

  getAllFeedback(): Observable<any> {
    return this.http.get(`${this.apiUrl}/all`);
  }

  getOwnFeedback(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/ownSubmission/${userId}`);
  }

  getSubmissionFeedback(submissionId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/submission/${submissionId}`);
  }

  downloadMarks(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/download-marks/${userId}`);
  }

  updateFeedback(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${id}`, data);
  }

  deleteFeedback(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}


