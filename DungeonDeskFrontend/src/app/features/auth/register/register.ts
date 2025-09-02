import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RegisterService } from '../../../services/auth/register-service';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [RouterModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private loginService = inject(RegisterService);
  private formBuilderService = inject(NonNullableFormBuilder);

  protected form = this.formBuilderService.group({
    email: this.formBuilderService.control('', {
      validators: [Validators.required, Validators.email],
    }),
    password: this.formBuilderService.control('', {
      validators: [Validators.required, Validators.minLength(6)],
    }),
    passwordConfirmation: this.formBuilderService.control('', {
      validators: [Validators.required, Validators.minLength(6)],
    }),
    username: this.formBuilderService.control('', {
      validators: [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        Validators.pattern('^[a-zA-Z0-9_]+$'),
      ],
    }),
    name: this.formBuilderService.control('', {
      validators: [Validators.required],
    }),
  });

  protected onSubmit() {
    console.log(this.form.controls);

    if (this.form.valid) {
      const { email, password, passwordConfirmation, username, name } = this.form.value;
      if (password != passwordConfirmation) {
        this.form.controls['passwordConfirmation'].setErrors({ mismatch: true });
        console.log('Password and confirmation do not match');
        return;
      }
      if (email && password && passwordConfirmation && username && name) {
        this.loginService
          .register(email, password, passwordConfirmation, username, name)
          .subscribe({
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
