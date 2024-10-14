// auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private loginService: LoginService, private router: Router) { }

  canActivate(): boolean {
    // Allow the navigation if the user is logged in
    if (this.loginService.isLoggedIn()) {
      return true;
    }
    // Return false to prevent navigation
    return false;
  }
}



