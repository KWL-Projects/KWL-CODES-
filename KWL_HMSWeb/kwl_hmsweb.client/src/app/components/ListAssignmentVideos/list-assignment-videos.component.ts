import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VideoService } from '../../services/video.service'; // Adjust the path as necessary
import { Submission } from '../../models/submission.model'; // Adjust the path as necessary

@Component({
  selector: 'app-list-assignment-videos',
  templateUrl: './list-assignment-videos.component.html',
  styleUrls: ['./list-assignment-videos.component.css'] // Add your styles here
})
export class ListAssignmentVideos implements OnInit {
  assignmentId: number;
  submissions: Submission[] = [];
  errorMessage: string | null = null;

  constructor(private route: ActivatedRoute, private videoService: VideoService, private router: Router) {
    this.assignmentId = +this.route.snapshot.paramMap.get('id')!; // Fetching assignment ID from route parameters
  }

  ngOnInit(): void {
    this.loadSubmissions();
  }

  loadSubmissions(): void {
    this.videoService.viewAssignmentVideos(this.assignmentId).subscribe(
      (data: Submission[]) => {
        this.submissions = data;
      },
      (error) => {
        this.errorMessage = 'Error loading submissions: ' + error.message;
      }
    );
  }

  watchVideo(videoPath: string): void {
    // Logic to watch or download the video
    window.open(videoPath, '_blank'); // Opens the video in a new tab
  }

  goBack(): void {
    this.router.navigate(['/list-assignments']); // Navigate back to the list assignments page
  }
}



