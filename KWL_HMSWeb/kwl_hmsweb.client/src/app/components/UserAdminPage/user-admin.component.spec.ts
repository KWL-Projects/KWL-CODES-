import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { UserAdminPage } from './user-admin.component';
import { UserService } from '../../services/user.service';
import { of, throwError } from 'rxjs';

class MockUserService {
  getAllUsers() {
    return of([{ user_id: 1, login_id: 1, username: 'testuser', password: 'testpass', user_firstname: 'Test', user_surname: 'User', user_type: 'Admin' }]);
  }

  deleteUser(userId: number) {
    return of(null);
  }
}

class MockRouter {
  navigate = jasmine.createSpy('navigate');
}

describe('UserAdminPage', () => {
  let component: UserAdminPage;
  let fixture: ComponentFixture<UserAdminPage>;
  let userService: UserService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAdminPage],
      providers: [
        { provide: UserService, useClass: MockUserService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserAdminPage);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on initialization', () => {
    component.ngOnInit();
    expect(component.users.length).toBe(1);
  });

  it('should select user', () => {
    component.onSelectUser(1);
    expect(component.selectedUserId).toBe(1);
  });

  it('should edit selected user', () => {
    component.onSelectUser(1);
    component.editUser();
    expect(router.navigate).toHaveBeenCalledWith(['/edit-user', 1]);
  });

  it('should not edit user when none is selected', () => {
    component.editUser();
    expect(component.errorMessage).toBe('Please select a user to edit.');
  });

  it('should confirm deletion of user', () => {
    component.confirmDeleteUser(1);
    expect(component.selectedUserId).toBe(1);
    expect(component.confirmDelete).toBeTrue();
  });

  it('should cancel deletion', () => {
    component.confirmDeleteUser(1);
    component.cancelDelete();
    expect(component.confirmDelete).toBeFalse();
    expect(component.selectedUserId).toBeNull();
  });

  it('should delete selected user', () => {
    component.onSelectUser(1);
    component.deleteUser();
    expect(component.selectedUserId).toBeNull();
    expect(component.confirmDelete).toBeFalse(); // Ensure confirmation dialog is closed
  });

  it('should show error message when deleting without selection', () => {
    component.deleteUser();
    expect(component.errorMessage).toBe('Please select a user to delete.');
  });

  it('should handle errors when loading users', () => {
    spyOn(userService, 'getAllUsers').and.returnValue(throwError(() => new Error('Error loading users')));
    component.loadUsers();
    expect(component.errorMessage).toBe('Error loading users: Error loading users');
  });

  it('should handle errors when deleting users', () => {
    spyOn(userService, 'deleteUser').and.returnValue(throwError(() => new Error('Error deleting user')));
    component.onSelectUser(1);
    component.deleteUser();
    expect(component.errorMessage).toBe('Error deleting user: Error deleting user');
  });
});











