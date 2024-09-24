import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
//import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { UserAdminComponent } from './components/user-administration/user-administration.component';
import { ListAssignmentsComponent } from './components/list-assignments/list-assignments.component';
import { CreateAssignmentComponent } from './components/create-assignment/create-assignment.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  //{ path: 'home', component: AppComponent },
  { path: 'login', component: LoginComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'user-administration', component: UserAdminComponent },
  { path: 'list-assignments', component: ListAssignmentsComponent },
  { path: 'create-assignment', component: CreateAssignmentComponent },

  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }




