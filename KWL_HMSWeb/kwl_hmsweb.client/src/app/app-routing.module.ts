import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { UserAdministrationComponent } from './components/user-administration/user-administration.component';
import { ListAssignmentsComponent } from './components/list-assignments/list-assignments.component';
import { CreateAssignmentComponent } from './components/create-assignment/create-assignment.component';
import { ListAssignmentVideosComponent } from './components/list-assignment-videos/list-assignment-videos.component';
import { ProvideFeedbackComponent } from './components/provide-feedback/provide-feedback.component';
import { AppComponent } from './app.component'; // Import AppComponent

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' }, // Redirect to AppComponent by default
  { path: 'login', component: LoginComponent },
  { path: 'landing', component: LandingComponent }, // Optional: Keep this if you want a landing page
  { path: 'user-administration', component: UserAdministrationComponent },
  { path: 'list-assignments', component: ListAssignmentsComponent },
  { path: 'create-assignment', component: CreateAssignmentComponent },
  { path: 'list-assignment-videos', component: ListAssignmentVideosComponent },
  { path: 'provide-feedback', component: ProvideFeedbackComponent },
  // Wildcard route for undefined routes
  { path: '**', redirectTo: '/home' } // Redirect to AppComponent for any undefined route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }








