import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs'; // <--- Import 'of' and 'Observable'
import { delay } from 'rxjs/operators'; // <--- Import 'delay'
import { HistoryDto, StudentDashboardItem } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root',
})
export class Llmservice {
  getDashboard(filters: string[]): Observable<StudentDashboardItem[]> {
    // 1. Сначала определим учебные планы для каждой группы
const TASKS_PI_21 = [
  { id: 1, name: "Lab 1: Git Basics" },
  { id: 2, name: "Lab 2: Angular Components" },
  { id: 3, name: "Lab 3: Services & DI" },
  { id: 4, name: "Lab 4: Routing" },
  { id: 5, name: "Lab 5: Forms" },
  { id: 6, name: "Lab 6: HTTP Client" },
  { id: 7, name: "Lab 7: RxJS Basics" },
  { id: 8, name: "Lab 8: Final Project" }
];

const TASKS_IS_22 = [
  { id: 10, name: "DB 1: SQL Basics" },
  { id: 11, name: "DB 2: Joins & Unions" },
  { id: 12, name: "DB 3: Normalization" },
  { id: 13, name: "DB 4: Transactions" },
  { id: 14, name: "DB 5: NoSQL Introduction" }
];

const TASKS_DI_23 = [
  { id: 20, name: "UX 1: Figma Basics" },
  { id: 21, name: "UX 2: Color Theory" },
  { id: 22, name: "UX 3: Typography" },
  { id: 23, name: "UX 4: Prototyping" }
];

// 2. Теперь сами студенты
const data = [
  // --- ГРУППА ПИ-21 ---

  // 1. Отличник (Сдал всё)
  {
    id: 101,
    fio: "Смирнов Алексей Владимирович",
    groupName: "ПИ-21",
    allGroupTasks: TASKS_PI_21, // <--- Полный список
    tasksHistory: [
      { record_id: 1001, assignment_id: 1, task_name: "Lab 1: Git Basics", is_completed: true, date_str: "01.09.2025", completed_at: "2025-09-01T10:00:00" },
      { record_id: 1002, assignment_id: 2, task_name: "Lab 2: Angular Components", is_completed: true, date_str: "05.09.2025", completed_at: "2025-09-05T12:30:00" },
      { record_id: 1003, assignment_id: 3, task_name: "Lab 3: Services & DI", is_completed: true, date_str: "10.09.2025", completed_at: "2025-09-10T14:15:00" },
      { record_id: 1004, assignment_id: 4, task_name: "Lab 4: Routing", is_completed: true, date_str: "15.09.2025", completed_at: "2025-09-15T09:00:00" },
      { record_id: 1005, assignment_id: 5, task_name: "Lab 5: Forms", is_completed: true, date_str: "20.09.2025", completed_at: "2025-09-20T16:45:00" },
      { record_id: 1006, assignment_id: 6, task_name: "Lab 6: HTTP Client", is_completed: true, date_str: "25.09.2025", completed_at: "2025-09-25T11:20:00" },
      { record_id: 1007, assignment_id: 7, task_name: "Lab 7: RxJS Basics", is_completed: true, date_str: "01.10.2025", completed_at: "2025-10-01T13:10:00" },
      { record_id: 1008, assignment_id: 8, task_name: "Lab 8: Final Project", is_completed: true, date_str: "10.10.2025", completed_at: "2025-10-10T15:00:00" },
    ]
  },

  // 2. Середнячок (Долги по последним темам)
  {
    id: 102,
    fio: "Кузнецова Мария Игоревна",
    groupName: "ПИ-21",
    allGroupTasks: TASKS_PI_21, // <--- Полный список, хотя сдала не всё
    tasksHistory: [
      { record_id: 2001, assignment_id: 1, task_name: "Lab 1: Git Basics", is_completed: true, date_str: "02.09.2025", completed_at: "2025-09-02T11:00:00" },
      { record_id: 2002, assignment_id: 2, task_name: "Lab 2: Angular Components", is_completed: true, date_str: "07.09.2025", completed_at: "2025-09-07T13:45:00" },
      { record_id: 2003, assignment_id: 3, task_name: "Lab 3: Services & DI", is_completed: true, date_str: "12.09.2025", completed_at: "2025-09-12T10:30:00" },
      { record_id: 2004, assignment_id: 4, task_name: "Lab 4: Routing", is_completed: false, date_str: "18.09.2025", completed_at: "" }, 
      { record_id: 2005, assignment_id: 5, task_name: "Lab 5: Forms", is_completed: false, date_str: "25.09.2025", completed_at: "" }
    ]
  },

  // 3. Отстающий (Сдал только начало)
  {
    id: 103,
    fio: "Попов Дмитрий Сергеевич",
    groupName: "ПИ-21",
    allGroupTasks: TASKS_PI_21,
    tasksHistory: [
      { record_id: 3001, assignment_id: 1, task_name: "Lab 1: Git Basics", is_completed: true, date_str: "05.09.2025", completed_at: "2025-09-05T09:15:00" },
      { record_id: 3002, assignment_id: 2, task_name: "Lab 2: Angular Components", is_completed: false, date_str: "15.09.2025", completed_at: "" }
    ]
  },

  // 10. "Вернувшийся" (Странные даты, но список задач тот же)
  {
    id: 110,
    fio: "Морозова Екатерина Ильинична",
    groupName: "ПИ-21",
    allGroupTasks: TASKS_PI_21,
    tasksHistory: [
      { record_id: 1101, assignment_id: 1, task_name: "Lab 1: Git Basics", is_completed: true, date_str: "01.02.2025", completed_at: "2025-02-01T10:00:00" },
      { record_id: 1102, assignment_id: 2, task_name: "Lab 2: Angular Components", is_completed: false, date_str: "01.09.2025", completed_at: "" }
    ]
  },

  // --- ГРУППА ИС-22 ---

  // 4. Отличник
  {
    id: 104,
    fio: "Соколов Иван Петрович",
    groupName: "ИС-22",
    allGroupTasks: TASKS_IS_22, // <--- Свой список для ИС
    tasksHistory: [
      { record_id: 4001, assignment_id: 10, task_name: "DB 1: SQL Basics", is_completed: true, date_str: "01.09.2025", completed_at: "2025-09-02T10:00:00" },
      { record_id: 4002, assignment_id: 11, task_name: "DB 2: Joins & Unions", is_completed: true, date_str: "08.09.2025", completed_at: "2025-09-09T11:30:00" },
      { record_id: 4003, assignment_id: 12, task_name: "DB 3: Normalization", is_completed: true, date_str: "15.09.2025", completed_at: "2025-09-16T14:20:00" },
      { record_id: 4004, assignment_id: 13, task_name: "DB 4: Transactions", is_completed: true, date_str: "22.09.2025", completed_at: "2025-09-23T16:00:00" },
      { record_id: 4005, assignment_id: 14, task_name: "DB 5: NoSQL Introduction", is_completed: true, date_str: "29.09.2025", completed_at: "2025-09-30T09:45:00" }
    ]
  },

  // 5. Почти отличник
  {
    id: 105,
    fio: "Михайлов Андрей Дмитриевич",
    groupName: "ИС-22",
    allGroupTasks: TASKS_IS_22,
    tasksHistory: [
      { record_id: 5001, assignment_id: 10, task_name: "DB 1: SQL Basics", is_completed: true, date_str: "02.09.2025", completed_at: "2025-09-03T12:00:00" },
      { record_id: 5002, assignment_id: 11, task_name: "DB 2: Joins & Unions", is_completed: true, date_str: "10.09.2025", completed_at: "2025-09-11T10:15:00" },
      { record_id: 5003, assignment_id: 12, task_name: "DB 3: Normalization", is_completed: false, date_str: "20.09.2025", completed_at: "" }
    ]
  },

  // 6. Должник
  {
    id: 106,
    fio: "Новикова Елена Сергеевна",
    groupName: "ИС-22",
    allGroupTasks: TASKS_IS_22,
    tasksHistory: [
      { record_id: 6001, assignment_id: 10, task_name: "DB 1: SQL Basics", is_completed: false, date_str: "05.09.2025", completed_at: "" }
    ]
  },

  // 7. Новенький
  {
    id: 107,
    fio: "Лебедев Максим Викторович",
    groupName: "ИС-22",
    allGroupTasks: TASKS_IS_22,
    tasksHistory: [] 
  },

  // --- ГРУППА ДИ-23 ---

  // 8. Дизайнер-отличник
  {
    id: 108,
    fio: "Васильева Ольга Андреевна",
    groupName: "ДИ-23",
    allGroupTasks: TASKS_DI_23,
    tasksHistory: [
      { record_id: 8001, assignment_id: 20, task_name: "UX 1: Figma Basics", is_completed: true, date_str: "01.09.2025", completed_at: "2025-09-01T15:00:00" },
      { record_id: 8002, assignment_id: 21, task_name: "UX 2: Color Theory", is_completed: true, date_str: "08.09.2025", completed_at: "2025-09-08T16:30:00" },
      { record_id: 8003, assignment_id: 22, task_name: "UX 3: Typography", is_completed: true, date_str: "15.09.2025", completed_at: "2025-09-15T14:45:00" },
      { record_id: 8004, assignment_id: 23, task_name: "UX 4: Prototyping", is_completed: true, date_str: "22.09.2025", completed_at: "2025-09-22T11:00:00" }
    ]
  },

  // 9. Дизайнер отстающий
  {
    id: 109,
    fio: "Федоров Николай Павлович",
    groupName: "ДИ-23",
    allGroupTasks: TASKS_DI_23,
    tasksHistory: [
      { record_id: 9001, assignment_id: 20, task_name: "UX 1: Figma Basics", is_completed: true, date_str: "03.09.2025", completed_at: "2025-09-03T13:20:00" },
      { record_id: 9002, assignment_id: 21, task_name: "UX 2: Color Theory", is_completed: false, date_str: "10.09.2025", completed_at: "" },
      { record_id: 9003, assignment_id: 22, task_name: "UX 3: Typography", is_completed: false, date_str: "17.09.2025", completed_at: "" }
    ]
  }
];
  return of(data).pipe(delay(1000));
  }
  createTask(task: HistoryDto): Observable<number> {
    return of(task.record_id).pipe(delay(500));
  }
}
