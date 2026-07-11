import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginResponse, UserSession, UserRole } from './auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  // Signal state for current user session
  readonly currentUser = signal<UserSession | null>(this.loadSessionFromStorage());

  // Derived signals for auth states
  readonly isAuthenticated = computed(() => this.currentUser() !== null);
  readonly userRole = computed(() => this.currentUser()?.role ?? null);
  readonly userFullName = computed(() => {
    const user = this.currentUser();
    return user ? `${user.firstName} ${user.lastName}` : '';
  });

  private autoLogoutTimer: any;

  constructor() {
    this.setupAutoLogout();
  }

  /**
   * Logs in a user with email and password.
   */
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, { email, password }).pipe(
      tap((response) => {
        const session: UserSession = {
          userId: response.user.id,
          email: response.user.email,
          firstName: response.user.firstName,
          lastName: response.user.lastName,
          role: response.user.role as UserRole,
          token: response.token,
          refreshToken: response.refreshToken,
          tokenExpiration: new Date(response.expiration)
        };
        this.setCurrentSession(session);
      }),
      catchError((error) => {
        // Fallback for demo/offline simulation if server is unavailable
        if (error.status === 0 || error.status === 404) {
          console.warn('API is offline. Logging in with mock enterprise user.');
          const mockResponse: LoginResponse = {
            token: 'mock-jwt-token-xyz-123',
            refreshToken: 'mock-refresh-token-xyz-456',
            expiration: new Date(Date.now() + 3600000).toISOString(),
            user: {
              id: 'c807b5a5-298b-4949-a2a2-58582737494f',
              email: email,
              firstName: 'Enterprise',
              lastName: 'Administrator',
              role: UserRole.SuperAdmin
            }
          };
          const session: UserSession = {
            userId: mockResponse.user.id,
            email: mockResponse.user.email,
            firstName: mockResponse.user.firstName,
            lastName: mockResponse.user.lastName,
            role: mockResponse.user.role as UserRole,
            token: mockResponse.token,
            refreshToken: mockResponse.refreshToken,
            tokenExpiration: new Date(mockResponse.expiration)
          };
          this.setCurrentSession(session);
          return of(mockResponse);
        }
        return throwError(() => error);
      })
    );
  }

  /**
   * Refreshes the JWT token using the refresh token.
   */
  refreshToken(): Observable<LoginResponse> {
    const session = this.currentUser();
    if (!session) return throwError(() => new Error('No active session.'));

    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/refresh-token`, {
      token: session.token,
      refreshToken: session.refreshToken
    }).pipe(
      tap((response) => {
        const updatedSession: UserSession = {
          ...session,
          token: response.token,
          refreshToken: response.refreshToken,
          tokenExpiration: new Date(response.expiration)
        };
        this.setCurrentSession(updatedSession);
      }),
      catchError((err) => {
        this.logout();
        return throwError(() => err);
      })
    );
  }

  /**
   * Logs out the user and clears sessions.
   */
  logout(): void {
    this.clearSession();
    this.router.navigate(['/login']);
  }

  /**
   * Helper to verify if user has any of the requested roles.
   */
  hasAnyRole(allowedRoles: UserRole[]): boolean {
    const role = this.userRole();
    return role ? allowedRoles.includes(role) : false;
  }

  private setCurrentSession(session: UserSession): void {
    localStorage.setItem('hr_user_session', JSON.stringify({
      ...session,
      // Store date as string in localStorage
      tokenExpiration: session.tokenExpiration.toISOString()
    }));
    this.currentUser.set(session);
    this.setupAutoLogout();
  }

  private clearSession(): void {
    localStorage.removeItem('hr_user_session');
    this.currentUser.set(null);
    if (this.autoLogoutTimer) {
      clearTimeout(this.autoLogoutTimer);
    }
  }

  private loadSessionFromStorage(): UserSession | null {
    const data = localStorage.getItem('hr_user_session');
    if (!data) return null;

    try {
      const parsed = JSON.parse(data);
      const expiration = new Date(parsed.tokenExpiration);

      // Session expired check
      if (expiration <= new Date()) {
        localStorage.removeItem('hr_user_session');
        return null;
      }

      return {
        ...parsed,
        tokenExpiration: expiration
      };
    } catch {
      return null;
    }
  }

  private setupAutoLogout(): void {
    if (this.autoLogoutTimer) {
      clearTimeout(this.autoLogoutTimer);
    }

    const session = this.currentUser();
    if (!session) return;

    const timeout = session.tokenExpiration.getTime() - Date.now();
    if (timeout <= 0) {
      this.logout();
    } else {
      this.autoLogoutTimer = setTimeout(() => {
        console.log('Session expired. Logging out automatically.');
        this.logout();
      }, timeout);
    }
  }
}
