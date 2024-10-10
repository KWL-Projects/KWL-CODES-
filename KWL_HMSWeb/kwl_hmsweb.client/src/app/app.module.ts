import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Import your components
import { HomePage } from './components/HomePage/home.component';
import { LoginPage } from './components/LoginPage/login.component';
import { LandingPage } from './components/LandingPage/landing.component';
import { UserAdminPage } from './components/UserAdminPage/user-admin.component';
import { ListAssignments } from './components/ListAssignments/list-assignments.component';
import { CreateAssignment } from './components/CreateAssignment/create-assignment.component';
import { ListAssignmentVideos } from './components/ListAssignmentVideos/list-assignment-videos.component';
import { WatchVideoFeedback } from './components/WatchVideoFeedback/watch-video-feedback.component';

// Service imports
//import './services/auth.service';
import { LoginService } from './services/login.service';
import { UserService } from './services/user.service';
import { AssignmentService } from './services/assignment.service';
import { SubmissionService } from './services/submission.service';
import { FeedbackService } from './services/feedback.service';
import { VideoService } from './services/video.service';

// Other imports for environment and JWT if required
import { JwtModule } from '@auth0/angular-jwt';

// Function to get token for JWT
export function tokenGetter() {
  return localStorage.getItem('jwtToken');
}

@NgModule({
  declarations: [
    AppComponent,
    HomePage,
    LoginPage,
    LandingPage,
    UserAdminPage,
    ListAssignments,
    CreateAssignment,
    ListAssignmentVideos,
    WatchVideoFeedback
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:4200'], // Adjust based on your environment
        disallowedRoutes: ['http://localhost:4200/api/login/authenticate'] // Add any routes that should bypass JWT token checks
      }
    })
  ],
  providers: [
    LoginService,
    UserService,
    AssignmentService,
    SubmissionService,
    FeedbackService,
    VideoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
