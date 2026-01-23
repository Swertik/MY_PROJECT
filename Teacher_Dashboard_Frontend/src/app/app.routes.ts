import { Routes } from '@angular/router';
import { Auth } from './pages/auth/auth';
import { Dashboard } from './pages/dashboard/dashboard';
import { AdminPanel } from './pages/admin-panel/admin-panel';
import { adminGuard } from './guard/admin-guard';
import { authGuard } from './guard/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Auth },
  { 
    path: 'dashboard', 
    component: Dashboard,
    canActivate: [authGuard]
  },
  { 
    path: 'admin', 
    component: AdminPanel,
    canActivate: [adminGuard]
  },
];
