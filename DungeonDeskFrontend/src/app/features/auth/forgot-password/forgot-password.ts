import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  imports: [],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css',
})
export class ForgotPassword {
  private code: string = '';
  protected formularyShowing: 'email' | 'code' | 'success' = 'email';
  constructor(@Inject(ActivatedRoute) readonly route: ActivatedRoute) {
    this.code = route.snapshot.queryParamMap.get('code') || '';
  }

  resendCode() {
    throw new Error('Method not implemented.');
  }
}
