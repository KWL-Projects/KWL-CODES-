import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ListAssignmentsComponent } from './components/list-assignments/list-assignments.component';
import { UserAdministrationComponent } from './components/user-administration/user-administration.component';
import { CreateAssignmentComponent } from './components/create-assignment/create-assignment.component';
import { ListAssignmentVideosComponent } from './components/list-assignment-videos/list-assignment-videos.component';
import { ProvideFeedbackComponent } from './components/provide-feedback/provide-feedback.component';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LandingComponent,
    UserAdministrationComponent,
    ListAssignmentsComponent,
    CreateAssignmentComponent,
    ListAssignmentVideosComponent,
    ProvideFeedbackComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
