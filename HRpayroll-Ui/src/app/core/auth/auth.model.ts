export enum UserRole {
  SuperAdmin = 'Super Admin',
  HRAdmin = 'HR Admin',
  PayrollManager = 'Payroll Manager',
  Manager = 'Manager',
  Recruiter = 'Recruiter',
  Employee = 'Employee'
}

export interface UserSession {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  token: string;
  refreshToken: string;
  tokenExpiration: Date;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  expiration: string;
  user: {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
  };
}
