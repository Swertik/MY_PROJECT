import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  data = [{id:1, name: "Имя", group: "Группа", completed_tasks: [1,2,3]}]
}
