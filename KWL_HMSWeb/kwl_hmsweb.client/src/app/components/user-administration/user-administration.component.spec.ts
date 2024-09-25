import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAdministrationComponent } from './user-administration.component';

describe('UserAdministrationComponent', () => {
    type NewType = UserAdministrationComponent;

  let component: NewType;
  let fixture: ComponentFixture<UserAdministrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAdministrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAdministrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
