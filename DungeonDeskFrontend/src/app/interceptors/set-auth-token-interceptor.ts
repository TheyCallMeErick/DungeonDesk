import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoggedInUserStore } from '../services/auth/stores/logged-in-user-store';
import { TokenStorageService } from '../services/auth/token-storage-service';

export const setAuthTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const loggedInUserStoreService = inject(LoggedInUserStore);

  if (!loggedInUserStoreService.isLoggedIn()) {
    return next(req)
  }
  const tokenStorageService = inject(TokenStorageService);
  const token = tokenStorageService.getAccessToken();
  const newRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  })
  return next(newRequest);
};
