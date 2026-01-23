import { Component, input, output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-history-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './history-modal.html',
  styleUrls: ['./history-modal.css']
})
export class HistoryModal {
  student = input.required<any>();
  
  close = output<void>();
  deleteRecord = output<number>(); // Эмитим ID записи, которую надо удалить

  // Вычисляем перевернутый список для отображения (новые сверху)
  reversedHistory = computed(() => {
    return [...this.student().tasksHistory].reverse();
  });

  // Логика 24 часов
  canDelete(recordId: number): boolean {
    const ONE_DAY_MS = 24 * 60 * 60 * 1000;
    // record_id у нас это timestamp (Date.now())
    return (Date.now() - recordId) < ONE_DAY_MS;
  }

  onDelete(recordId: number) {
    // Подтверждение лучше спросить прямо тут
    if (confirm('Удалить эту запись из истории?')) {
      this.deleteRecord.emit(recordId);
    }
  }
}