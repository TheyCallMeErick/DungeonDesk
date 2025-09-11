import { inject, provideAppInitializer } from "@angular/core";
import { AuthService } from "../services/auth/auth-service";
import { TokenStorageService } from "../services/auth/token-storage-service";
import { LoggedInUserStore } from "../services/auth/stores/logged-in-user-store";
import { of, switchMap, tap } from "rxjs";
import User from "../types/interfaces/user";

export function provideLoggedInUser() {
  return provideAppInitializer(() => {
    const tokenStorageService = inject(TokenStorageService)

    if (!tokenStorageService.hasAccessToken()) {
      return of();
    }

    const authService = inject(AuthService)
    const loggedInUserStoreService = inject(LoggedInUserStore)

    const token = tokenStorageService.getAccessToken() as string

    return authService.refreshAccessToken(token).pipe(
      tap((response: any) => tokenStorageService.set(response.access_token, response.refresh_token)),
      switchMap((response: any) => authService.getCurrentUserByAccessToken(response.access_token)),
      tap((user: User) => loggedInUserStoreService.setUser(user))
    )
  });
}
