import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListAssignmentVideosComponent } from './list-assignment-videos.component';

describe('ListAssignmentVideosComponent', () => {
  let component: ListAssignmentVideosComponent;
  let fixture: ComponentFixture<ListAssignmentVideosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListAssignmentVideosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListAssignmentVideosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
