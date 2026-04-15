import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth';

export const adminGuard: CanActivateFn = () => {

  const authService = inject(Auth);
  const router = inject(Router);

  // 🔐 verifica se é admin
  if (authService.getUserRole() === 'Admin') {
    return true;
  }

  // 🚫 bloqueia acesso
  router.navigate(['/home']);
  return false;
};