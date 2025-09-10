import { inject, Injectable } from '@angular/core';
import { LocalStorageToken } from '../../features/tokens/local-storage-token';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  private readonly accessTokenKey: string = 'access-token';
  private readonly refreshTokenKey: string = 'refresh-token';

  localStorageToken = inject(LocalStorageToken);

  set(accessToken: string, refreshToken: string) {
    this.localStorageToken.setItem(this.accessTokenKey, accessToken)
    this.localStorageToken.setItem(this.refreshTokenKey, refreshToken)
  }
}
