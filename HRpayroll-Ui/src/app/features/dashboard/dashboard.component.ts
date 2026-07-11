import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../core/auth/auth.service';

interface StatCard {
  title: string;
  value: string;
  icon: string;
  color: string;
  change: string;
  isPositive: boolean;
}

interface ActivityLog {
  id: string;
  title: string;
  user: string;
  time: string;
  icon: string;
  badgeClass: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, MatDividerModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  authService = inject(AuthService);

  readonly stats = signal<StatCard[]>([
    { title: 'Total Employees', value: '342', icon: 'people', color: '#0284c7', change: '+12% this month', isPositive: true },
    { title: 'Present Today', value: '318', icon: 'check_circle', color: '#22c55e', change: '93% attendance rate', isPositive: true },
    { title: 'Absent Today', value: '24', icon: 'cancel', color: '#ef4444', change: '8 on approved leaves', isPositive: false },
    { title: 'Open Positions', value: '9', icon: 'work', color: '#a855f7', change: '3 active hiring pipelines', isPositive: true }
  ]);

  readonly activities = signal<ActivityLog[]>([
    { id: '1', title: 'New Employee Onboarded', user: 'Sarah Jenkins (Engineering)', time: '10 mins ago', icon: 'person_add', badgeClass: 'badge-success' },
    { id: '2', title: 'Leave Approved', user: 'David Miller (Product Management)', time: '1 hour ago', icon: 'holiday_village', badgeClass: 'badge-info' },
    { id: '3', title: 'Payroll Processed', user: 'June Month Salary cycle finalized', time: '3 hours ago', icon: 'payments', badgeClass: 'badge-primary' }
  ]);
}
