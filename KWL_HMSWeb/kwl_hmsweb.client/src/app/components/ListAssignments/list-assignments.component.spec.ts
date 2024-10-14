import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListAssignments } from './list-assignments.component';
import { AssignmentService } from '../../services/assignment.service'; // Adjust path as necessary
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('ListAssignments', () => {
  let component: ListAssignments;
  let fixture: ComponentFixture<ListAssignments>;
  let mockAssignmentService: jasmine.SpyObj<AssignmentService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockAssignmentService = jasmine.createSpyObj('AssignmentService', ['getAllAssignments', 'deleteAssignment']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [ListAssignments],
      providers: [
        { provide: AssignmentService, useValue: mockAssignmentService },
        { provide: Router, useValue: mockRouter }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(ListAssignments);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load assignments on init', () => {
    const mockAssignments = [
      { assignment_id: 1, subject_id: 1, assignment_name: 'Assignment 1', assignment_description: 'Description 1', due_date: new Date() },
      { assignment_id: 2, subject_id: 2, assignment_name: 'Assignment 2', assignment_description: 'Description 2', due_date: new Date() }
    ];
    mockAssignmentService.getAllAssignments.and.returnValue(of(mockAssignments));

    component.ngOnInit();

    expect(component.assignments).toEqual(mockAssignments);
  });

  it('should handle error when loading assignments', () => {
    mockAssignmentService.getAllAssignments.and.returnValue(throwError({ message: 'Error' }));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Error loading assignments: Error');
  });

  it('should navigate to assignment details page with assignment_id', () => {
    const assignment_id = 1;
    component.viewAssignment(assignment_id);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/assignment-details', assignment_id]);
  });

  it('should delete assignment and reload assignments', () => {
    const assignment_id = 1;
    mockAssignmentService.deleteAssignment.and.returnValue(of(null));
    spyOn(component, 'loadAssignments').and.callThrough();

    component.deleteAssignment(assignment_id);

    expect(mockAssignmentService.deleteAssignment).toHaveBeenCalledWith(assignment_id);
    expect(component.loadAssignments).toHaveBeenCalled();
  });

  it('should handle error when deleting assignment', () => {
    const assignment_id = 1;
    mockAssignmentService.deleteAssignment.and.returnValue(throwError({ message: 'Error' }));

    component.deleteAssignment(assignment_id);

    expect(component.errorMessage).toBe('Error deleting assignment: Error');
  });
});



