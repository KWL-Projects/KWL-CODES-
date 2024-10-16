import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service'; // Adjust path as necessary

@Component({
  selector: 'app-user-admin',
  templateUrl: './user-admin.component.html',
  styleUrls: ['./user-admin.component.css'] // Add your styles here
})
export class UserAdminPage implements OnInit {
  users: any[] = []; // Array to hold users (type is 'any' to avoid using model)
  selectedUserId: number | null = null; // To hold selected user ID for edit/delete
  selectedUser: any | null = null; // Holds the selected user
  errorMessage: string | null = null;
  confirmDelete: boolean = false; // To hold the confirmation state

  creatingUser: boolean = false;
  // Define new user structure inline instead of using a model
  newUser: any = {
    user_id: 0,
    login_id: 0,
    username: '',
    password: '',
    user_firstname: '',
    user_surname: '',
    user_type: 'Student' // Default to Student
  };

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  onSubmit() {
    console.log('Form submitted');
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe(
      (data) => {
        this.users = data; // Now data should be the array of users
        console.log('Users:', this.users);
      },
      (error) => {
        console.error('Error loading users:', error);
      }
    );
  }

  // Show the create user form
  toggleCreateUser(): void {
    this.creatingUser = !this.creatingUser; // Toggle visibility of the create form
    if (this.creatingUser) {
      this.newUser = {
        user_id: 0,
        login_id: 0,
        username: '',
        password: '',
        user_firstname: '',
        user_surname: '',
        user_type: 'Student' // Reset form with default values
      };
    }
  }

  createUser(): void {
    if (this.newUser.username && this.newUser.password && this.newUser.user_type) {
      this.userService.registerUser(this.newUser).subscribe(
        (response: any) => {
          if (response.token) {
            console.log('User registered and JWT token received:', response.token);
            this.loadUsers(); // Reload users after creation
            this.creatingUser = false; // Hide the creation form
            this.errorMessage = null; // Clear error message
          }
        },
        (error) => {
          this.errorMessage = 'Error creating user: ' + error.message;
        }
      );
    } else {
      this.errorMessage = 'Please fill in all required fields.';
    }
  }

  onSelectUser(userId: number): void {
    this.selectedUserId = userId; // Set the selected user ID
    this.selectedUser = this.users.find(user => user.user_id === userId) || null; // Get selected user details for editing
  }

  editUser(): void {
    if (this.selectedUserId !== null) {
      // Navigate to the edit user page with selected user ID
      this.router.navigate(['/edit-user', this.selectedUserId]);
    } else {
      this.errorMessage = 'Please select a user to edit.';
    }
  }

  confirmDeleteUser(userId: number): void {
    this.selectedUserId = userId; // Set the selected user ID
    this.confirmDelete = true; // Show confirmation dialog
  }

  cancelDelete(): void {
    this.confirmDelete = false; // Hide confirmation dialog
    this.selectedUserId = null; // Clear selected user ID
  }

  deleteUser(): void {
    if (this.selectedUserId !== null) {
      this.userService.deleteUser(this.selectedUserId).subscribe(
        () => {
          this.loadUsers(); // Reload users after deletion
          this.selectedUserId = null; // Clear selected user ID
          this.confirmDelete = false; // Hide confirmation dialog
        },
        (error) => {
          this.errorMessage = 'Error deleting user: ' + error.message;
        }
      );
    } else {
      this.errorMessage = 'Please select a user to delete.';
    }
  }
}







