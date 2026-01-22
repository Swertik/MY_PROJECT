import { Routes } from '@angular/router';
import { Auth } from './pages/auth/auth';
import { Dashboard } from './pages/dashboard/dashboard';
import { AdminPanel } from './pages/admin-panel/admin-panel';
import { adminGuard } from './guard/admin-guard';

export const routes: Routes = [
    { 
    path: 'admin', 
    component: AdminPanel,
    canActivate: [adminGuard] // <-- Подключаем здесь
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' }, // По умолчанию идем на логин
  { path: 'login', component: Auth },
  { path: 'dashboard', component: Dashboard },
];
