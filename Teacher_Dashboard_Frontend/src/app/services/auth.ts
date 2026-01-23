import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  confirmPassword: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5050/api';
  private tokenKey = 'auth_token';
  private userKey = 'user_info';

  currentUserRole = signal<string>("guest");
  currentUsername = signal<string>("");
  isAuthenticated = signal<boolean>(false);

  private authSubject = new BehaviorSubject<boolean>(this.hasValidToken());

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/login`, credentials)
      .pipe(
        tap(response => {
          this.saveAuthData(response);
        })
      );
  }

  register(userData: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/register`, userData)
      .pipe(
        tap(response => {
          this.saveAuthData(response);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUserRole.set("guest");
    this.currentUsername.set("");
    this.isAuthenticated.set(false);
    this.authSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAdmin(): boolean {
    return this.currentUserRole() === 'Admin';
  }

  isTeacher(): boolean {
    return this.currentUserRole() === 'Teacher';
  }

  private saveAuthData(response: AuthResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.userKey, JSON.stringify({
      username: response.username,
      role: response.role,
      expiresAt: response.expiresAt
    }));

    this.currentUserRole.set(response.role);
    this.currentUsername.set(response.username);
    this.isAuthenticated.set(true);
    this.authSubject.next(true);
  }

  private loadUserFromStorage(): void {
    const userInfo = localStorage.getItem(this.userKey);
    const token = localStorage.getItem(this.tokenKey);

    if (userInfo && token && this.hasValidToken()) {
      const user = JSON.parse(userInfo);
      this.currentUserRole.set(user.role);
      this.currentUsername.set(user.username);
      this.isAuthenticated.set(true);
      this.authSubject.next(true);
    }
  }

  private hasValidToken(): boolean {
    const userInfo = localStorage.getItem(this.userKey);
    const token = localStorage.getItem(this.tokenKey);

    if (!userInfo || !token) {
      return false;
    }

    try {
      const user = JSON.parse(userInfo);
      const expiresAt = new Date(user.expiresAt);
      return expiresAt > new Date();
    } catch {
      return false;
    }
  }

  getAuthStatus(): Observable<boolean> {
    return this.authSubject.asObservable();
  }
}
