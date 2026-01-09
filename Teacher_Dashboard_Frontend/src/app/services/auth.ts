import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  currentUserRole = signal<string>("guest")

  isAdmin(): boolean {
    return this.currentUserRole() == 'admin'
  }
}
