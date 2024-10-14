import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { LoginPage } from './login.component';
import { LoginService } from '../../services/login.service'; // Adjust the path as necessary
import { ReactiveFormsModule } from '@angular/forms';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { of, throwError } from 'rxjs';

describe('LoginPage', () => {
  let component: LoginPage;
  let fixture: ComponentFixture<LoginPage>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockAuthService: jasmine.SpyObj<LoginService>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockAuthService = jasmine.createSpyObj('LoginService', ['authenticate']);

    await TestBed.configureTestingModule({
      declarations: [LoginPage],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: LoginService, useValue: mockAuthService }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call login method on submit', () => {
    component.loginForm.setValue({ username: 'testuser', password: 'testpass' });

    mockAuthService.authenticate.and.returnValue(of({ success: true })); // Mock successful login
    component.onLogin();

    expect(mockAuthService.authenticate).toHaveBeenCalledWith('testuser', 'testpass');
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/landing']);
  });

  it('should set error message on login failure', () => {
    component.loginForm.setValue({ username: 'testuser', password: 'testpass' });

    mockAuthService.authenticate.and.returnValue(of({ success: false, message: 'Invalid credentials' })); // Mock login failure
    component.onLogin();

    expect(component.errorMessage).toBe('Invalid credentials');
  });

  it('should handle service error', () => {
    component.loginForm.setValue({ username: 'testuser', password: 'testpass' });

    mockAuthService.authenticate.and.returnValue(throwError('Service error')); // Mock service error
    component.onLogin();

    expect(component.errorMessage).toBe('Login failed. Please try again.');
  });
});




