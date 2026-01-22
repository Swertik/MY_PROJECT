import { Component, computed, ElementRef, HostListener, inject, OnInit, signal, ViewChild } from '@angular/core';
import { SearchOption, StudentDashboardItem } from '../../models/dashboard.model';
import { Llmservice } from '../../services/llmservice';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { ModalForm } from "../../modal-forms/modal-form/modal-form";
import { HistoryModal } from "../../modal-forms/history-modal/history-modal";
import { ModalMassTask } from "../../modal-forms/modal-mass-task/modal-mass-task";


export type SortCriteria = 'name' | 'performance';
export type SortDirection = 'asc' | 'desc';

@Component({
  selector: 'app-dashboard',
  imports: [FormsModule, NgClass, ModalForm, HistoryModal, ModalMassTask],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private raw_data = signal<StudentDashboardItem[]>([]);

  currentInput = signal("");
  searchChips = signal<SearchOption[]>([]);

  isDropdownOpen = signal(false);
  focusedIndex = signal(-1);
  sortCriteria = signal<SortCriteria>('name');
  sortDirection = signal<SortDirection>('asc')


  lmmService = inject(Llmservice)

  @ViewChild('searchInput') searchInputRef!: ElementRef<HTMLInputElement>;

  @HostListener('window:keydown', ['$event'])
  handleGlobalKeyboardEvent(event: KeyboardEvent) {

    const activeElement = document.activeElement as HTMLElement;
    const isTypingSomewhere = activeElement.tagName === 'INPUT' || activeElement.tagName === 'TEXTAREA';
    
    if (!isTypingSomewhere && (event.key === 'Enter' || event.key === '/')) {
      event.preventDefault();
      this.searchInputRef.nativeElement.focus();
    }
  }

  allAvailableOptions = computed<SearchOption[]>(() => {
    const students = this.raw_data();
    const options: SearchOption[] = [];
    const addedGroups = new Set<string>();
    const typeOrder: Record<string, number> = {
  'group': 1,
  'student': 2,
  'status': 3
};

    options.push({ label: 'Должники', type: 'status', value: 'debtor' });
    options.push({ label: 'Сдали всё', type: 'status', value: 'completed' });

    students.forEach(s => {

      if (!addedGroups.has(s.groupName)) {
        options.push({ label: s.groupName, type: 'group', value: s.groupName });
        addedGroups.add(s.groupName);
      }
      

      options.push({ label: s.fio, type: 'student', value: s.fio });
    });

    return options.sort((a, b) => {
  return typeOrder[a.type] - typeOrder[b.type];
});
  });

  getCategoryName(type: string): string {
    switch (type) {
      case 'group':
        return 'Группы';
      case 'student':
        return 'Студенты';
      case 'status':
        return 'Статусы и фильтры';
      default:
        return 'Прочее';
    }
  }

  getProgressBarColor(student_tasks: number, group_tasks: number): string {
    const percentage = (student_tasks / group_tasks) * 100;
    if (percentage >= 80) return 'green';
    if (percentage >= 50) return 'orange';
    return 'red';
  }


  dropdownOptions = computed(() => {
    const input = this.currentInput().toLowerCase();
    const currentChips = this.searchChips().map(c => c.label);
    
    return this.allAvailableOptions().filter(opt => 
      opt.label.toLowerCase().includes(input) &&
      !currentChips.includes(opt.label)
    );
  });

  filteredData = computed(() => {
    const students = this.raw_data();
    const chips = this.searchChips();

    const criteria = this.sortCriteria();
    const direction = this.sortDirection() === 'asc' ? 1 : -1;

    var result = students.sort((a,b) => {
      if (criteria == 'name'){
        return a.fio.localeCompare(b.fio) * direction
      }
      else if (criteria == 'performance') {
        const progressA = this.calculateProgress(a);
        const progressB = this.calculateProgress(b);
        return (progressA - progressB) * direction;
      }

      return 0
    })

    if (chips.length === 0) return students;

    const groupFilters = chips
      .filter(c => c.type === 'group')
      .map(c => c.value);

    const statusFilters = chips
      .filter(c => c.type === 'status')
      .map(c => c.value);

    const nameFilters = chips
      .filter(c => c.type === 'student') 
      .map(c => c.value.toLowerCase());

    result = result.filter(student => {
      
      const matchesGroup = groupFilters.length === 0 || 
                           groupFilters.includes(student.groupName);

      const matchesName = nameFilters.length === 0 || 
                          nameFilters.some(name => student.fio.toLowerCase().includes(name));

      const matchesStatus = statusFilters.length === 0 || 
                            statusFilters.some(statusVal => this.checkStatus(student, statusVal));

      return matchesGroup && matchesName && matchesStatus;
    });

    return result
  });

  calculateProgress(student: any): number {
    if (!student.allGroupTasks || student.allGroupTasks.length === 0) return 0;
    
    const completedCount = student.tasksHistory.filter((t: any) => t.is_completed).length;
    return (completedCount / student.allGroupTasks.length) * 100;
  }

  toggleSort(criteria: SortCriteria) {
    if (this.sortCriteria() === criteria) {
      this.sortDirection.update(d => d === 'asc' ? 'desc' : 'asc');
    } else {
      this.sortCriteria.set(criteria);
      this.sortDirection.set(criteria === 'performance' ? 'desc' : 'asc');
    }
  }

  checkStatus(student: StudentDashboardItem, statusValue: string): boolean {
    switch (statusValue) {
      case 'debtor': 
        return student.tasksHistory.length === 0 || student.tasksHistory.some(t => !t.is_completed);
      
      case 'completed':
        return student.tasksHistory.length !== 0 && student.tasksHistory.every(t => t.is_completed);
        
      default:
        return true;
    }
  }

  selectOption(option: SearchOption) {
    this.searchChips.update(chips => [...chips, option]);
    this.currentInput.set('');
    this.focusedIndex.set(-1);
  }

  removeChip(index: number) {
    this.searchChips.update(chips => chips.filter((_, i) => i !== index));
  }

  onInputFocus() {
    this.isDropdownOpen.set(true);
    this.focusedIndex.set(0);
  }

  onInputBlur() {
    setTimeout(() => {
      this.isDropdownOpen.set(false);
    }, 200);
  }

  ngOnInit(): void {
    this.loadData()
  }

  loadData() {
    this.lmmService.getDashboard([]).subscribe({
      next: (data) => this.raw_data.set(data),
      error: (err) => console.error(err)
    });
  }

  scrollToOption(index: number) {
    const list = document.querySelector('.dropdown-list');
    const item = list?.children[index] as HTMLElement;
    if (item && list) {
      item.scrollIntoView({ block: 'nearest' }); 
    }
  }

  onKeyDown (event: KeyboardEvent){
    console.log(1);
    if (!this.isDropdownOpen() || this.dropdownOptions().length == 0) return;

    const options = this.dropdownOptions();
    const currentIndex = this.focusedIndex();

    switch (event.key){
      case "ArrowDown":
      
        event.preventDefault();

        const nextIndex = (currentIndex + 1) % options.length;
        this.focusedIndex.set(nextIndex);
        this.scrollToOption(nextIndex);
        break;
      case "ArrowUp":
        event.preventDefault();

        const prevIndex = (currentIndex - 1 + options.length) % options.length;
        this.focusedIndex.set(prevIndex);
        this.scrollToOption(prevIndex);
        break;
      case "Enter":
        event.preventDefault();

        if (currentIndex >= 0 && currentIndex < options.length){
          this.selectOption(options[currentIndex])
        }
        break;
      case "Escape":
        this.isDropdownOpen.set(false);
        this.searchInputRef.nativeElement.blur();
        break;
    }
  }

  activeStudentForEdit = signal<any | null>(null);

  // Метод открытия (вызывается из кнопки "+" в шаблоне)
  openTaskModal(student: any) {
    this.activeStudentForEdit.set(student);
  }

  // Метод закрытия
  closeTaskModal() {
    this.activeStudentForEdit.set(null);
  }

  // ГЛАВНАЯ ЛОГИКА: Сохранение и обновление данных
  onAssignTask(result: { taskId: number, isCompleted: boolean }) {
    const student = this.activeStudentForEdit();
    if (!student) return;

    // Находим само задание по ID в списке задач группы
    const taskDefinition = student.allGroupTasks.find((t: any) => t.id === result.taskId);

    // Создаем новую запись истории
    const newRecord = {
      record_id: Date.now(), // Генерируем ID
      assignment_id: result.taskId,
      task_name: taskDefinition?.name || 'Unknown Task',
      is_completed: result.isCompleted,
      date_str: new Date().toLocaleDateString('ru-RU'), // Текущая дата
      completed_at: result.isCompleted ? new Date().toISOString() : ''
    };

    // ОБНОВЛЯЕМ СИГНАЛ (Immutable update)
    this.raw_data.update(currentData => {
      return currentData.map(s => {
        // Ищем нашего студента
        if (s.id === student.id) {
          return {
            ...s,
            // Добавляем запись в его массив tasksHistory
            tasksHistory: [...s.tasksHistory, newRecord] 
          };
        }
        return s;
      });
    });

    this.closeTaskModal();

    this.lmmService.createTask(newRecord).subscribe({
      next: (recordId) => {
        console.log(`Задание создано с record_id: ${recordId}`);}
      });
  }

  activeStudentForHistory = signal<any | null>(null);

  openHistory(student: any) {
    this.activeStudentForHistory.set(student);
  }

  closeHistory() {
    this.activeStudentForHistory.set(null);
  }

  // Метод удаления (принимает ID записи)
  onDeleteHistoryRecord(recordId: number) {
    const student = this.activeStudentForHistory();
    if (!student) return;

    // Обновляем главный массив данных (Immutable update)
    this.raw_data.update(data => {
      return data.map(s => {
        if (s.id === student.id) {
          // Вычитаем удаленную запись из массива tasksHistory
          return {
            ...s,
            tasksHistory: s.tasksHistory.filter((t: any) => t.record_id !== recordId)
          };
        }
        return s;
      });
    });

    // Важно: так как student в сигнале activeStudentForHistory - это ссылка на старый объект,
    // нам нужно обновить и его, чтобы модалка перерисовалась мгновенно.
    // Мы берем свежие данные из raw_data:
    const updatedStudent = this.raw_data().find(s => s.id === student.id);
    this.activeStudentForHistory.set(updatedStudent);

  }

  isMassModalOpen = signal(false);

  // 1. Вычисляем список задач для массовой выдачи.
  // Логика: берем задачи первого студента из фильтра (предполагаем, что фильтр обычно по группе).
  // Или собираем уникальные задачи со всех (если фильтр смешанный).
  commonTasks = computed(() => {
    const students = this.filteredData();
    if (students.length === 0) return [];
    
    // Упрощение: берем список задач первого студента.
    // В реальном проекте тут нужно пересечение (Intersection) массивов задач.
    return students[0].allGroupTasks;
  });

  openMassModal() {
    this.isMassModalOpen.set(true);
  }

  closeMassModal() {
    this.isMassModalOpen.set(false);
  }

  // 2. МАССОВОЕ ОБНОВЛЕНИЕ
  onMassAssign(payload: { taskId: number, isCompleted: boolean }) {
    // Получаем ID всех студентов, которые сейчас на экране
    const targetStudentIds = new Set(this.filteredData().map(s => s.id));
    
    // Находим определение задачи (имя и т.д.)
    // Берем из commonTasks
    const taskDef = this.commonTasks().find((t: any) => t.id === payload.taskId);
    if (!taskDef) return;

    const baseRecord = {
      assignment_id: payload.taskId,
      task_name: taskDef.name,
      is_completed: payload.isCompleted,
      date_str: new Date().toLocaleDateString('ru-RU')
    };

    // ОБНОВЛЯЕМ ВСЁ ОДНИМ MAРОМ (Immutable)
    this.raw_data.update(allData => {
      return allData.map(student => {
        // Если студент есть в отфильтрованном списке
        if (targetStudentIds.has(student.id)) {
          
          // Проверяем, нет ли уже такого задания (чтобы не дублировать)
          // Если можно дублировать - проверку убираем.
          const alreadyHas = student.tasksHistory.some((t: any) => t.assignment_id === payload.taskId);
          if (alreadyHas && !payload.isCompleted) {
             // Если уже есть и мы не закрываем его - пропускаем (или обновляем?)
             // Допустим, просто добавляем новую запись
          }

          return {
            ...student,
            tasksHistory: [
              ...student.tasksHistory,
              {
                ...baseRecord,
                record_id: Date.now() + Math.random(), // Уникальный ID для каждой записи
                completed_at: payload.isCompleted ? new Date().toISOString() : ''
              }
            ]
          };
        }
        // Если студент не в фильтре - не трогаем
        return student;
      });
    });

    this.closeMassModal();
    // Тут можно добавить Toast уведомление: "Задание выдано 25 студентам"
  }
  
}
