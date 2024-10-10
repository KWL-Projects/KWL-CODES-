import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-administration',
  templateUrl: './user-administration.component.html',
  styleUrls: ['./user-administration.component.css']
})
export class UserAdministrationComponent implements OnInit {
  users = [
    { login_id: '1', user_first_name: 'John', user_surname: 'Doe', user_type: 'A' },
    { login_id: '2', user_first_name: 'Jane', user_surname: 'Smith', user_type: 'U' }
  ];
  selectedUser: any = null;
  showAddForm: boolean = false;
  newUser: any = { login_id: '', user_first_name: '', user_surname: '', user_type: '' };

  constructor() { }

  ngOnInit(): void { }

  selectUser(user: any) {
    this.selectedUser = { ...user };
  }

  updateUser() {
    const index = this.users.findIndex(u => u.login_id === this.selectedUser.login_id);
    if (index !== -1) {
      this.users[index] = { ...this.selectedUser };
      this.selectedUser = null;
    }
  }

  deleteUser(login_id: string) {
    this.users = this.users.filter(user => user.login_id !== login_id);
  }

  showAddUserForm() {
    this.showAddForm = true;
  }

  addUser() {
    this.users.push({ ...this.newUser });
    this.newUser = { login_id: '', user_first_name: '', user_surname: '', user_type: '' };
    this.showAddForm = false;
  }
}
