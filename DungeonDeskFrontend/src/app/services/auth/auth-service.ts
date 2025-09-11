import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable, of } from 'rxjs';
import User from '../../types/interfaces/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/${environment.apiVersion}/auth`;

  login(email: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, {
      email,
      password,
    });
  }

  getCurrentUserByAccessToken(accessToken: string): Observable<User> {
    return this.http.get(`${this.apiUrl}/current-user`, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    }) as Observable<User>;
  }

  refreshAccessToken(token: string) {
    return of({ token: 'fake-token' });
  }
}
