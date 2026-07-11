import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: '',
    loadComponent: () => import('./layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'employees',
        loadComponent: () => import('./features/employees/employees.component').then(m => m.EmployeesComponent)
      },
      {
        path: 'departments',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'designations',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'attendance',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'leaves',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'payroll',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'recruitment',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'performance',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      },
      {
        path: 'assets',
        loadComponent: () => import('./shared/components/feature-placeholder/feature-placeholder.component').then(m => m.FeaturePlaceholderComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
