import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { StudentDashboardItem } from '../../models/dashboard.model';
import { Llmservice } from '../../services/llmservice';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private raw_data = signal<StudentDashboardItem[]>([]);

  lmsService = inject(Llmservice)

  table_data = computed(() => {
    return this.raw_data();
  })

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
