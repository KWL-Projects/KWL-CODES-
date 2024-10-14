import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { LandingPage } from './landing.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('LandingPage', () => {
  let component: LandingPage;
  let fixture: ComponentFixture<LandingPage>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [LandingPage],
      providers: [
        { provide: Router, useValue: mockRouter }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(LandingPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to User Administration page', () => {
    component.navigateToUserAdministration();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/user-administration']);
  });

  it('should navigate to List Assignments page', () => {
    component.navigateToListAssignments();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/list-assignments']);
  });

  it('should navigate to Create Assignment page', () => {
    component.navigateToCreateAssignment();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/create-assignment']);
  });
});


