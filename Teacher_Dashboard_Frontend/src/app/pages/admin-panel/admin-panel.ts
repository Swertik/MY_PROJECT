import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';

interface GroupStatistics {
  groupName: string;
  totalStudents: number;
  totalAssignments: number;
  completedAssignments: number;
  completionPercentage: number;
}

@Component({
  selector: 'app-admin-panel',
  imports: [FormsModule, NgClass],
  templateUrl: './admin-panel.html',
  styleUrl: './admin-panel.css',
})
export class AdminPanel implements OnInit {
  authService = inject(AuthService);
  router = inject(Router);
  http = inject(HttpClient);

  private apiUrl = 'http://localhost:5050/api';
  
  statistics = signal<GroupStatistics[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<GroupStatistics[]>(`${this.apiUrl}/CompletedAssignment/group-statistics`)
      .subscribe({
        next: (data) => {
          this.statistics.set(data);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Error loading statistics:', error);
          this.errorMessage.set('Ошибка при загрузке статистики');
          this.isLoading.set(false);
        }
      });
  }

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getProgressColor(percentage: number): string {
    if (percentage >= 80) return 'high';
    if (percentage >= 50) return 'medium';
    return 'low';
  }
}
