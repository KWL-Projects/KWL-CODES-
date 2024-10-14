import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the import path as necessary

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  private apiUrl = `${environment.apiUrl}/api/video`;

  constructor(private http: HttpClient) { }

  // Uploads the video file (form data) to the server, which stores it in Azure Blob Storage
  uploadVideo(videoData: FormData): Observable<any> {
    // Ensure `videoData` is a FormData object that contains the file to be uploaded
    return this.http.post(`${this.apiUrl}/upload`, videoData);
  }

  // Fetches a list of all video files from Blob Storage (retrieved through the server API)
  getAllVideos(): Observable<any> {
    return this.http.get(`${this.apiUrl}/all`);
  }

  viewAssignmentVideos(assignmentId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/assignment/${assignmentId}`);
  }

  // Downloads a video file from Blob Storage (via the server) and expects a blob response (binary data)
  downloadSubmission(submission_id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/download-stream/${submission_id}`, { responseType: 'blob' });
  }

  // Downloads a video file from Blob Storage (via the server) and expects a blob response (binary data)
  downloadVideo(fileName: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/download/${fileName}`, { responseType: 'blob' });
  }
}




