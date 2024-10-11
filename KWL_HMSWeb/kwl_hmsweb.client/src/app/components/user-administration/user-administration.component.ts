import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-administration',
  templateUrl: './user-administration.component.html',
  styleUrls: ['./user-administration.component.css']
})
export class UserAdministrationComponent implements OnInit {
  users: any[] = [];
  selectedUser: any = null;
  showAddForm: boolean = false;
  newUser: any = { login_id: '', user_first_name: '', user_surname: '', user_type: '' };

  constructor(private location: Location, private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.http.get<any>('https://localhost:7074/api/user/all')
      .subscribe(response => {
        console.log('API response:', response); // Log the response
        if (response.message === 'Success') {
          this.users = response.data;
          console.log('Users:', this.users); // Log the users array
        } else {
          console.error('Failed to fetch users:', response.message);
        }
      }, error => {
        console.error('Error fetching users:', error);
      });
  }

  selectUser(user: any) {
    this.selectedUser = { ...user };
  }

  updateUser() {
    this.http.put<any>(`https://localhost:7074/api/user/update/${this.selectedUser.user_id}`, this.selectedUser)
      .subscribe(response => {
        console.log('Update user response:', response); // Log the response
        if (response.message === 'User updated successfully') {
          const index = this.users.findIndex(u => u.user_id === this.selectedUser.user_id);
          if (index !== -1) {
            this.users[index] = { ...this.selectedUser };
          }
          this.selectedUser = null;
        } else {
          console.error('Failed to update user:', response.message);
        }
      }, error => {
        console.error('Error updating user:', error);
      });
  }

  deleteUser(login_id: string) {
    this.http.delete<any>(`https://localhost:7074/api/user/delete/${login_id}`)
      .subscribe(response => {
        console.log('Delete user response:', response); // Log the response
        if (response.message === 'User deleted successfully') {
          this.users = this.users.filter(user => user.login_id !== login_id);
        } else {
          console.error('Failed to delete user:', response.message);
        }
      }, error => {
        console.error('Error deleting user:', error);
      });
  }

  showAddUserForm() {
    this.showAddForm = true;
  }

  addUser() {
    this.http.post<any>('https://localhost:7074/api/user/create', this.newUser)
      .subscribe(response => {
        console.log('Add user response:', response); // Log the response
        if (response.message === 'User created successfully') {
          this.users.push(response.data);
          this.newUser = { login_id: '', user_first_name: '', user_surname: '', user_type: '' };
          this.showAddForm = false;
        } else {
          console.error('Failed to add user:', response.message);
        }
      }, error => {
        console.error('Error adding user:', error);
      });
  }

  goBack() {
    this.location.back();
  }
}
