import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
//import { AuthGuard } from './services/auth.guard';

// Importing the components
import { HomePage } from './components/HomePage/home.component';
import { LoginPage } from './components/LoginPage/login.component';
import { LandingPage } from './components/LandingPage/landing.component';
import { UserAdminPage } from './components/UserAdminPage/user-admin.component';
import { ListAssignments } from './components/ListAssignments/list-assignments.component';
import { CreateAssignment } from './components/CreateAssignment/create-assignment.component';
import { ListAssignmentVideos } from './components/ListAssignmentVideos/list-assignment-videos.component';
import { WatchVideoFeedback } from './components/WatchVideoFeedback/watch-video-feedback.component';

// Defining the routes
const routes: Routes = [
  { path: 'home', component: HomePage }, // Home Page
  { path: 'login', component: LoginPage }, // Login Page
  { path: 'landing', component: LandingPage }, // Landing Page
  { path: 'user-admin', component: UserAdminPage }, // User Administration Page
  { path: 'list-assignments', component: ListAssignments }, // List Assignments
  { path: 'create-assignment', component: CreateAssignment }, // Create Assignment
  { path: 'list-assignment-videos', component: ListAssignmentVideos }, // List Assignment Videos
  { path: 'watch-video-feedback', component: WatchVideoFeedback }, // Watch Video and Provide Feedback
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' } // Wildcard route for a 404 page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }










