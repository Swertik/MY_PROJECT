import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'; // <--- Import 'of' and 'Observable'
import { delay } from 'rxjs/operators'; // <--- Import 'delay'
import { HistoryDto, StudentDashboardItem } from '../models/dashboard.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class Llmservice {
  private apiUrl = 'http://localhost:5050/api';

  http = inject(HttpClient);

  getDashboard(): Observable<StudentDashboardItem[]> {
    var result = this.http.get<StudentDashboardItem[]>(`${this.apiUrl}/Dashboard`);
    console.log('Fetching dashboard data from API...', result);
    return result;
  }
  createTask(studentId: number, task: HistoryDto): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/CompletedAssignment/toggle`, {
      studentId: studentId,
      assignmentId: task.assignmentId,
      isCompleted: false
    });
  }
  deleteTask(recordId: number): Observable<boolean> {
    return of(true).pipe(delay(500));
  }

  createMassTask(studentIds: number[], taskId: number, isCompleted: boolean): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/CompletedAssignment/massCreate`, {
      studentIds: studentIds,
      assignmentId: taskId,
      isCompleted: isCompleted
    });
  }
}
