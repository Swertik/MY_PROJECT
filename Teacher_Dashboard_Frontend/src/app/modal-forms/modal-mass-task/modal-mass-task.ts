import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-modal-mass-task',
  imports: [CommonModule, FormsModule],
  templateUrl: './modal-mass-task.html',
  styleUrl: './modal-mass-task.css',
})
export class ModalMassTask {
  count = input.required<number>();
  availableTasks = input.required<any[]>(); // Список задач

  close = output<void>();
  save = output<{ taskId: number, isCompleted: boolean }>();

  selectedTaskId = signal<number | null>(null);
  isCompleted = signal(false);

  saveMass() {
    if (this.selectedTaskId()) {
      this.save.emit({
        taskId: this.selectedTaskId()!,
        isCompleted: this.isCompleted()
      });
    }
  }
}
