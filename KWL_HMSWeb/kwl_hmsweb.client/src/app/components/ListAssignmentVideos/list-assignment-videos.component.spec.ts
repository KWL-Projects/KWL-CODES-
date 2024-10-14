import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListAssignmentVideos } from './list-assignment-videos.component';
import { VideoService } from '../../services/video.service'; // Adjust path as necessary
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('ListAssignmentVideos', () => {
  let component: ListAssignmentVideos;
  let fixture: ComponentFixture<ListAssignmentVideos>;
  let mockVideoService: jasmine.SpyObj<VideoService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockActivatedRoute: any;

  beforeEach(async () => {
    mockVideoService = jasmine.createSpyObj('VideoService', ['viewAssignmentVideos']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockActivatedRoute = {
      snapshot: {
        paramMap: {
          get: (key: string) => {
            return '1'; // Return a mock assignment ID for testing
          }
        }
      }
    };

    await TestBed.configureTestingModule({
      declarations: [ListAssignmentVideos],
      providers: [
        { provide: VideoService, useValue: mockVideoService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(ListAssignmentVideos);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load submissions on init', () => {
    const mockSubmissions = [
      { submission_id: 1, assignment_id: 1, user_id: 1, submission_date: new Date(), submission_description: 'description1', video_path: 'path/to/video1.mp4' },
      { submission_id: 2, assignment_id: 2, user_id: 2, submission_date: new Date(), submission_description: 'description1', video_path: 'path/to/video2.mp4' }
    ];
    mockVideoService.viewAssignmentVideos.and.returnValue(of(mockSubmissions));

    component.ngOnInit();

    expect(component.submissions).toEqual(mockSubmissions);
  });

  it('should handle error when loading submissions', () => {
    mockVideoService.viewAssignmentVideos.and.returnValue(throwError({ message: 'Error' }));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Error loading submissions: Error');
  });

  it('should navigate back to assignments', () => {
    component.goBack();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/list-assignments']);
  });
});





