import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
//import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { UserAdministrationComponent } from './components/user-administration/user-administration.component';
import { ListAssignmentsComponent } from './components/list-assignments/list-assignments.component';
import { CreateAssignmentComponent } from './components/create-assignment/create-assignment.component';
import { ListAssignmentVideosComponent } from './components/list-assignment-videos/list-assignment-videos.component';
import { ProvideFeedbackComponent } from './components/provide-feedback/provide-feedback.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  //{ path: 'home', component: AppComponent },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'landing', component: LandingComponent },
  { path: '', redirectTo: '/landing', pathMatch: 'full' },
  { path: 'user-administration', component: UserAdministrationComponent },
  { path: '', redirectTo: '/user-administration', pathMatch: 'full' },
  { path: 'list-assignments', component: ListAssignmentsComponent },
  { path: '', redirectTo: '/list-assignments', pathMatch: 'full' },
  { path: 'create-assignment', component: CreateAssignmentComponent },
  { path: '', redirectTo: '/create-assignment', pathMatch: 'full' },
  { path: 'list-assignment-videos', component: ListAssignmentVideosComponent },
  { path: '', redirectTo: '/list-assignment-videos', pathMatch: 'full' },
  { path: 'provide-feedback', component: ProvideFeedbackComponent },
  { path: '', redirectTo: '/provide-feedback', pathMatch: 'full' },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }




