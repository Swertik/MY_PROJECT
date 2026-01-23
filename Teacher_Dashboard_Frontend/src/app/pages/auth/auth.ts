import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, LoginRequest, RegisterRequest } from '../../services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth',
  imports: [FormsModule, CommonModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class Auth {
  isLoginMode = signal(true);
  isLoading = signal(false);
  errorMessage = signal('');

  // Login form
  loginForm = signal<LoginRequest>({
    username: '',
    password: ''
  });

  // Register form
  registerForm = signal<RegisterRequest>({
    username: '',
    password: '',
    confirmPassword: '',
    role: 'Teacher'
  });

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  toggleMode(): void {
    this.isLoginMode.update(mode => !mode);
    this.errorMessage.set('');
  }

  onLogin(): void {
    const form = this.loginForm();
    
    if (!form.username || !form.password) {
      this.errorMessage.set('Заполните все поля');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService.login(form).subscribe({
      next: (response) => {
        console.log('Login successful:', response);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        console.error('Login error:', error);
        this.errorMessage.set(error.error?.message || 'Ошибка входа');
        this.isLoading.set(false);
      },
      complete: () => {
        this.isLoading.set(false);
      }
    });
  }

  onRegister(): void {
    const form = this.registerForm();
    
    if (!form.username || !form.password || !form.confirmPassword) {
      this.errorMessage.set('Заполните все поля');
      return;
    }

    if (form.password !== form.confirmPassword) {
      this.errorMessage.set('Пароли не совпадают');
      return;
    }

    if (form.password.length < 6) {
      this.errorMessage.set('Пароль должен содержать минимум 6 символов');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService.register(form).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        console.error('Registration error:', error);
        this.errorMessage.set(error.error?.message || 'Ошибка регистрации');
        this.isLoading.set(false);
      },
      complete: () => {
        this.isLoading.set(false);
      }
    });
  }

  updateLoginField(field: keyof LoginRequest, value: string): void {
    this.loginForm.update(form => ({ ...form, [field]: value }));
  }

  updateRegisterField(field: keyof RegisterRequest, value: string): void {
    this.registerForm.update(form => ({ ...form, [field]: value }));
  }
}
