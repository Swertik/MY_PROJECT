import { Component, computed, ElementRef, HostListener, inject, OnInit, signal, ViewChild } from '@angular/core';
import { SearchOption, StudentDashboardItem } from '../../models/dashboard.model';
import { Llmservice } from '../../services/llmservice';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  imports: [FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private raw_data = signal<StudentDashboardItem[]>([]);

  currentInput = signal("");
  searchChips = signal<SearchOption[]>([]);

  isDropdownOpen = signal(false);
  focusedIndex = signal(-1);

  lmsService = inject(Llmservice)

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

    return students.filter(student => {
      
      const matchesGroup = groupFilters.length === 0 || 
                           groupFilters.includes(student.groupName);

      const matchesName = nameFilters.length === 0 || 
                          nameFilters.some(name => student.fio.toLowerCase().includes(name));

      const matchesStatus = statusFilters.length === 0 || 
                            statusFilters.some(statusVal => this.checkStatus(student, statusVal));

      return matchesGroup && matchesName && matchesStatus;
    });
  });

  checkStatus(student: StudentDashboardItem, statusValue: string): boolean {
    switch (statusValue) {
      case 'debtor': 
        return student.tasksHistory.some(t => !t.is_completed);
      
      case 'completed':
        return student.tasksHistory.every(t => t.is_completed);
        
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
    this.lmsService.getDashboard([]).subscribe({
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


  
}
