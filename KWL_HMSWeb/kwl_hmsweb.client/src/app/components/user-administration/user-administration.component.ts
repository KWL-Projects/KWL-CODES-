// src/app/components/user-admin/user-admin.component.ts
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-admin',
  templateUrl: './user-admin.component.html',
  styleUrls: ['./user-admin.component.css']
})
export class UserAdminComponent implements OnInit {
  users = [
    { id: 1, username: 'user1', role: 'Admin' },
    { id: 2, username: 'user2', role: 'User' }
  ];

  constructor() { }

  ngOnInit(): void { }

  editUser(user: any) {
    // Implement edit user logic here
    console.log('Edit user:', user);
  }

  deleteUser(userId: number) {
    // Implement delete user logic here
    console.log('Delete user with ID:', userId);
  }
}
