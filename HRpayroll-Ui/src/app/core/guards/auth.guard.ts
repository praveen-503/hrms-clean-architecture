import { inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { UserRole } from '../auth/auth.model';

/**
 * Route guard to check user login status and roles.
 */
export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    // Redirect to login page with returning URL parameters
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }

  // Check role authorization if specified
  const expectedRoles = route.data['roles'] as UserRole[];
  if (expectedRoles && expectedRoles.length > 0) {
    if (!authService.hasAnyRole(expectedRoles)) {
      router.navigate(['/access-denied']);
      return false;
    }
  }

  return true;
};
