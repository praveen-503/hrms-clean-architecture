import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

/**
 * Functional HTTP Interceptor to attach JWT token to all outbound API calls.
 */
export const jwtInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const authService = inject(AuthService);
  const session = authService.currentUser();

  if (session && session.token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${session.token}`
      }
    });
    return next(cloned);
  }

  return next(req);
};
