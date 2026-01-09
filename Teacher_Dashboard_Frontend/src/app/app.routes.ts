import { Routes } from '@angular/router';
import { Auth } from './auth/auth';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, // По умолчанию идем на логин
  { path: 'login', component: Auth },
  { path: 'dashboard', component: Dashboard },
];
