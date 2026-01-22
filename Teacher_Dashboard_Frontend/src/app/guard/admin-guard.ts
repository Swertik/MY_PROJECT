import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const isUserAdmin = authService.isAdmin();

  if (isUserAdmin) {
    return true;
  }

  return router.createUrlTree(['/dashboard']);

};
