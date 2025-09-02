import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/${environment.apiVersion}/player/create`;

  register(
    email: string,
    password: string,
    passwordConfirmation: string,
    username: string,
    name: string
  ) {
    return this.http.post(this.apiUrl, {
      email,
      password,
      passwordConfirmation,
      username,
      name,
    });
  }
}
