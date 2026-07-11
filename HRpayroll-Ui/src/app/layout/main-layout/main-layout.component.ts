import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../core/auth/auth.service';
import { UserRole } from '../../core/auth/auth.model';

interface MenuItem {
  label: string;
  route: string;
  icon: string;
  roles: UserRole[];
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    MatMenuModule,
    MatBadgeModule,
    MatTooltipModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent {
  authService = inject(AuthService);
  private router = inject(Router);

  // Sidebar collapse signal
  readonly isCollapsed = signal(false);

  // Dark theme signal
  readonly isDarkMode = signal(false);

  // Dynamic menu items configuration
  private readonly menuItemsList: MenuItem[] = [
    { label: 'Dashboard', route: '/dashboard', icon: 'dashboard', roles: Object.values(UserRole) },
    { label: 'Employees', route: '/employees', icon: 'people', roles: [UserRole.SuperAdmin, UserRole.HRAdmin, UserRole.Manager, UserRole.Recruiter] },
    { label: 'Departments', route: '/departments', icon: 'domain', roles: [UserRole.SuperAdmin, UserRole.HRAdmin, UserRole.Manager] },
    { label: 'Designations', route: '/designations', icon: 'badge', roles: [UserRole.SuperAdmin, UserRole.HRAdmin] },
    { label: 'Attendance', route: '/attendance', icon: 'event_available', roles: Object.values(UserRole) },
    { label: 'Leaves', route: '/leaves', icon: 'holiday_village', roles: Object.values(UserRole) },
    { label: 'Payroll', route: '/payroll', icon: 'payments', roles: [UserRole.SuperAdmin, UserRole.PayrollManager, UserRole.Employee] },
    { label: 'Recruitment', route: '/recruitment', icon: 'work', roles: [UserRole.SuperAdmin, UserRole.HRAdmin, UserRole.Recruiter] },
    { label: 'Performance', route: '/performance', icon: 'query_stats', roles: Object.values(UserRole) },
    { label: 'Assets', route: '/assets', icon: 'devices', roles: [UserRole.SuperAdmin, UserRole.HRAdmin, UserRole.Employee] }
  ];

  // Compute allowed menu items based on role
  readonly menuItems = computed(() => {
    const role = this.authService.userRole();
    if (!role) return [];
    return this.menuItemsList.filter(item => item.roles.includes(role));
  });

  toggleSidebar() {
    this.isCollapsed.update(val => !val);
  }

  toggleTheme() {
    this.isDarkMode.update(val => !val);
    if (this.isDarkMode()) {
      document.body.classList.add('dark-theme');
    } else {
      document.body.classList.remove('dark-theme');
    }
  }

  logout() {
    this.authService.logout();
  }
}
