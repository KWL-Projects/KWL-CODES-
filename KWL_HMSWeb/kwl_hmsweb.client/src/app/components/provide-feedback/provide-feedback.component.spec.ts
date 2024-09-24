import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvideFeedbackComponent } from './provide-feedback.component';

describe('ProvideFeedbackComponent', () => {
  let component: ProvideFeedbackComponent;
  let fixture: ComponentFixture<ProvideFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProvideFeedbackComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProvideFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
