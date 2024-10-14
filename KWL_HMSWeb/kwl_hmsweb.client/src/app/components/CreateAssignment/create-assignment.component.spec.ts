import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateAssignment } from './create-assignment.component';
import { AssignmentService } from '../../services/assignment.service'; // Adjust path as necessary
import { SubjectService } from '../../services/subject.service'; // Adjust path as necessary
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('CreateAssignment', () => {
  let component: CreateAssignment;
  let fixture: ComponentFixture<CreateAssignment>;
  let mockAssignmentService: jasmine.SpyObj<AssignmentService>;
  let mockSubjectService: jasmine.SpyObj<SubjectService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockAssignmentService = jasmine.createSpyObj('AssignmentService', ['createAssignment']);
    mockSubjectService = jasmine.createSpyObj('SubjectService', ['getSubjects']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [CreateAssignment],
      providers: [
        { provide: AssignmentService, useValue: mockAssignmentService },
        { provide: SubjectService, useValue: mockSubjectService },
        { provide: Router, useValue: mockRouter }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateAssignment);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load subjects on init', () => {
    const mockSubjects = [
      { subject_id: 1, user_id: 1, subject_name: 'Subject 1', subject_description: 'description1' },
      { subject_id: 2, user_id: 2, subject_name: 'Subject 2', subject_description: 'description2' }
    ];
    mockSubjectService.getAllSubjects.and.returnValue(of(mockSubjects));

    component.ngOnInit();

    expect(component.subjects).toEqual(mockSubjects);
  });

  it('should handle error when loading subjects', () => {
    mockSubjectService.getAllSubjects.and.returnValue(throwError({ message: 'Error' }));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Error loading subjects: Error');
  });

  it('should create an assignment and navigate', () => {
    mockAssignmentService.createAssignment.and.returnValue(of(null));

    component.assignment = {
      assignment_id: 1,
      assignment_name: 'New Assignment',
      assignment_description: 'Description',
      due_date: new Date(),
      subject_id: 1
    };

    component.createAssignment();

    expect(mockAssignmentService.createAssignment).toHaveBeenCalledWith(component.assignment);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/list-assignments']);
    expect(component.successMessage).toBe('Assignment created successfully!');
    expect(component.errorMessage).toBeNull();
  });

  it('should handle error when creating assignment', () => {
    mockAssignmentService.createAssignment.and.returnValue(throwError({ message: 'Error' }));

    component.createAssignment();

    expect(component.errorMessage).toBe('Error creating assignment: Error');
    expect(component.successMessage).toBeNull();
  });
});


