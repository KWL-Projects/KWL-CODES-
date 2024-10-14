import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WatchVideoFeedback } from './watch-video-feedback.component';
import { VideoService } from '../../services/video.service'; // Adjust path as necessary
import { FeedbackService } from '../../services/feedback.service'; // Adjust path as necessary
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('WatchVideoFeedback', () => {
  let component: WatchVideoFeedback;
  let fixture: ComponentFixture<WatchVideoFeedback>;
  let mockVideoService: jasmine.SpyObj<VideoService>;
  let mockFeedbackService: jasmine.SpyObj<FeedbackService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockActivatedRoute: any;

  beforeEach(async () => {
    mockVideoService = jasmine.createSpyObj('VideoService', ['downloadSubmission']);
    mockFeedbackService = jasmine.createSpyObj('FeedbackService', ['getSubmissionFeedback', 'submitFeedback']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockActivatedRoute = {
      snapshot: {
        paramMap: {
          get: (key: string) => {
            return '1'; // Return a mock submission ID for testing
          }
        }
      }
    };

    await TestBed.configureTestingModule({
      declarations: [WatchVideoFeedback],
      providers: [
        { provide: VideoService, useValue: mockVideoService },
        { provide: FeedbackService, useValue: mockFeedbackService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(WatchVideoFeedback);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load submission and feedback on init', () => {
    const mockSubmission = { submission_id: 1, assignment_id: 1, user_id: 1, submission_date: new Date(), submission_description: 'description1', video_path: 'path/to/video.mp4' };
    const mockFeedback = { feedback_id: 1, submission_id: 1, feedback: 'Great work!', mark_received: 90 };

    mockVideoService.downloadSubmission.and.returnValue(of(mockSubmission));
    mockFeedbackService.getSubmissionFeedback.and.returnValue(of(mockFeedback));

    component.ngOnInit();

    expect(component.submission).toEqual(mockSubmission);
    expect(component.feedback).toEqual(mockFeedback);
    expect(component.mark).toEqual(90);
    expect(component.comment).toEqual('Great work!');
  });

  it('should handle error when loading submission', () => {
    mockVideoService.downloadSubmission.and.returnValue(throwError({ message: 'Error' }));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Error loading submission: Error');
  });

  it('should handle error when loading feedback', () => {
    mockFeedbackService.getSubmissionFeedback.and.returnValue(throwError({ message: 'Error' }));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Error loading feedback: Error');
  });

  it('should submit feedback', () => {
    const feedbackData = { submission_id: 1, user_id: 'User1', feedback: 'Good job!', mark_received: 85 };
    mockFeedbackService.submitFeedback.and.returnValue(of({}));

    component.comment = 'Good job!';
    component.mark = 85;

    component.submitFeedback();

    expect(mockFeedbackService.submitFeedback).toHaveBeenCalledWith(feedbackData);
  });

  it('should navigate back to the list of assignment videos', () => {
    component.goBack();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/list-assignment-videos', component.submission?.assignment_id]);
  });
});



