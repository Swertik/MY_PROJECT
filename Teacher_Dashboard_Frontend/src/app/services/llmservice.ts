import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'; // <--- Import 'of' and 'Observable'
import { delay } from 'rxjs/operators'; // <--- Import 'delay'
import { StudentDashboardItem } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root',
})
export class Llmservice {
  getDashboard(filters: string[]): Observable<StudentDashboardItem[]> {
    var data = [
  {
    id: 101,
    fio: "Иванов Иван Иванович",
    groupName: "ПИ-21",
    allGroupTasks: [
      { id: 1, name: "Лабораторная 1: Git" },
      { id: 2, name: "Лабораторная 2: Angular Components" },
      { id: 3, name: "Лабораторная 3: Services" }
    ],
    tasksHistory: [
      {
        record_id: 501,
        assignment_id: 1,
        task_name: "Лабораторная 1: Git",
        is_completed: true,
        date_str: "01.09.2025",
        completed_at: "2025-09-01T14:30:00"
      },
      {
        record_id: 502,
        assignment_id: 2,
        task_name: "Лабораторная 2: Angular Components",
        is_completed: true,
        date_str: "15.09.2025",
        completed_at: "2025-09-15T16:45:00"
      },
      {
        record_id: 503,
        assignment_id: 3,
        task_name: "Лабораторная 3: Services",
        is_completed: true,
        date_str: "20.09.2025",
        completed_at: "2025-09-20T10:15:00"
      }
    ]
  },

  // 2. Середнячок (Одно задание в процессе, одно сдано)
  {
    id: 102,
    fio: "Петров Петр Петрович",
    groupName: "ПИ-21",
    allGroupTasks: [
      { id: 1, name: "Лабораторная 1: Git" },
      { id: 2, name: "Лабораторная 2: Angular Components" },
      { id: 3, name: "Лабораторная 3: Services" }
    ],
    tasksHistory: [
      {
        record_id: 601,
        assignment_id: 1,
        task_name: "Лабораторная 1: Git",
        is_completed: true,
        date_str: "05.09.2025",
        completed_at: "2025-09-05T12:00:00"
      },
      {
        record_id: 602,
        assignment_id: 2,
        task_name: "Лабораторная 2: Angular Components",
        is_completed: false, // <--- Еще не сдал
        date_str: "25.09.2025", // Планируемая дата или дата выдачи
        completed_at: "" // Пусто, так как не завершено
      }
    ]
  },

  // 3. Должник из другой группы (Ничего не сдал или мало данных)
  {
    id: 103,
    fio: "Сидорова Анна Сергеевна",
    groupName: "ИС-21", // Другая группа
    allGroupTasks: [
      { id: 5, name: "Базы данных: SQL Basics" },
      { id: 6, name: "Базы данных: Joins" }
    ],
    tasksHistory: [
      {
        record_id: 701,
        assignment_id: 5,
        task_name: "Базы данных: SQL Basics",
        is_completed: false,
        date_str: "01.10.2025",
        completed_at: ""
      }
    ]
  }
];
  return of(data).pipe(delay(1000));
  }
}
