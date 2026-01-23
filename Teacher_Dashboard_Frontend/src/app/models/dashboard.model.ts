export interface HistoryDto {
  recordId: number;
  assignmentId: number;
  taskName: string;
  isCompleted: boolean;
  completedAt: string | null;
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