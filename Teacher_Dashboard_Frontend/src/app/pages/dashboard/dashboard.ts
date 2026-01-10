import { Component, computed, inject, OnInit, signal } from '@angular/core';
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

  lmsService = inject(Llmservice)

  isDropdownOpen = signal(false);

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
  }

  removeChip(index: number) {
    this.searchChips.update(chips => chips.filter((_, i) => i !== index));
  }

  onInputFocus() {
    this.isDropdownOpen.set(true);
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
  
}
