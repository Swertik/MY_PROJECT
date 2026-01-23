export interface HistoryDto {
  record_id: number;
  assignment_id: number;
  task_name: string;
  is_completed: boolean;
  date_str: string;
  completed_at: string;
}

export interface TaskDto {
  id: number;
  name: string;
}

export interface StudentDashboardItem {
  id: number;
  fio: string;
  groupName: string;
  tasksHistory: HistoryDto[];
  allGroupTasks: TaskDto[];
}

export interface SearchOption {
  label: string;
  type: 'group' | 'student' | 'status';
  value: string;
}