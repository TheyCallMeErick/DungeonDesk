import { Component, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { LoginService } from '../../../services/auth/login-service';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

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
  private loginService = inject(LoginService);
  private formBuilderService = inject(NonNullableFormBuilder);

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
        this.loginService.login(email, password).subscribe({
          next: (response) => {
            console.log('Login successful', response);
          },
          error: (error) => {
            console.error('Login failed', error);
          },
        });
        return;
      }
    }
    this.form.markAllAsTouched();
  }
}
