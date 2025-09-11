import { inject } from '@angular/core';
import { CanActivateFn, RedirectCommand, Router } from '@angular/router';
import { LoggedInUserStore } from '../services/auth/stores/logged-in-user-store';

export const isAuthenticatedGuard: CanActivateFn = (route, state) => {
  const loggedInUserStoreService = inject(LoggedInUserStore);
  if (loggedInUserStoreService.isLoggedIn()) {
    return true;
  }
  const router = inject(Router);

  const urlTree = router.parseUrl('/auth/login');
  return new RedirectCommand(urlTree);
};
