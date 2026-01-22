import { Component, computed, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StudentDashboardItem } from '../../models/dashboard.model';

@Component({
  selector: 'app-modal-form',
  imports: [FormsModule, CommonModule],
  templateUrl: './modal-form.html',
  styleUrl: './modal-form.css',
})
export class ModalForm {

  student = input.required<StudentDashboardItem>();

  student_tasks = computed(() => {
    const student_tasks = this.student().tasksHistory;
    const group_tasks = this.student().allGroupTasks;

    const existingTaskIds = new Set(student_tasks.map((h: any) => h.assignment_id));

    return group_tasks.filter((task: any) => !existingTaskIds.has(task.id));
  })

  // Выходные события
  close = output<void>();
  save = output<{ taskId: number, isCompleted: boolean }>();

  // Состояние формы (Signals)
  selectedTaskId = signal<number | null>(null);
  isCompleted = signal(false);

  saveTask() {
    const taskId = this.selectedTaskId();
    if (taskId) {
      this.save.emit({ 
        taskId: taskId, 
        isCompleted: this.isCompleted() 
      });
    }
  }
}
