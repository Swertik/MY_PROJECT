import { Component, computed, ElementRef, HostListener, inject, OnInit, signal, ViewChild } from '@angular/core';
import { StudentDashboardItem } from '../../models/dashboard.model';
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
  searchChips = signal<string[]>([]);

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

  allAvailableOptions = computed(() => {
    const students = this.raw_data();
    const options = new Set<string>();

    students.forEach(s => {
      options.add(s.fio);
    });

    return Array.from(options).sort();
  });


  dropdownOptions = computed(() => {
    const input = this.currentInput().toLowerCase();
    const all = this.allAvailableOptions();
    const currentChips = this.searchChips();

    return all.filter(opt => 
      opt.toLowerCase().includes(input) &&

      !currentChips.includes(opt)
    );
  });

  filteredData = computed(() => {
    const raw_data = this.raw_data();
    const chips = this.searchChips().map(c => c.toLowerCase());

    if (chips.length == 0) return raw_data;
    var result = raw_data.filter(student => {
      const fullText = (student.fio + ' ' + student.groupName).toLowerCase();
      return chips.some(chip => fullText.includes(chip.toLowerCase()));
    });
    console.log(result);
    return result;

  })

  selectOption(option: string) {
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
