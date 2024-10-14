export interface User {
  user_id: number;
  login_id: number;
  username: string;
  password: string; 
  user_firstname: string; // User's first name
  user_surname: string; // User's surname
  user_type: 'Admin' | 'Lecturer' | 'Student'; // User type
}
