import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { TokenStorageService } from '../../../services/auth/token-storage-service';
import { AuthService } from '../../../services/auth/auth-service';
import { LoggedInUserStore } from '../../../services/auth/stores/logged-in-user-store';
import User from '../../../types/interfaces/user';
import { switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [
    ButtonModule,
    InputTextModule,
    FloatLabelModule,
    PasswordModule,
    RouterModule,
    ReactiveFormsModule,
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private formBuilderService = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private tokenStorageService = inject(TokenStorageService);
  private loggedInUserStoreService = inject(LoggedInUserStore);

  protected form = this.formBuilderService.group({
    email: this.formBuilderService.control('', {
      validators: [Validators.required, Validators.email],
    }),
    password: this.formBuilderService.control('', {
      validators: [Validators.required, Validators.minLength(6)],
    }),
  });

  protected onSubmit() {
    if (this.form.valid) {
      const { email, password } = this.form.value;
      if (email && password) {
        this.authService
          .login(email, password)
          .pipe(
            tap((response: any) =>
              this.tokenStorageService.set(response.access_token, response.refresh_token)
            ),
            switchMap((response: any) =>
              this.authService.getCurrentUserByAccessToken(response.access_token)
            ),
            tap((user: User) => this.loggedInUserStoreService.setUser(user))
          )
          .subscribe({
            next: (response: any) => {
              this.router.navigate(['tavern/quest-board']);
            },
            error: (response: HttpErrorResponse) => {
              if (response.status === 401) {
                this.form.setErrors({
                  wrongCredentials: true,
                });
              }
            },
          });
        return;
      }
    }
    this.form.markAllAsTouched();
  }
}
