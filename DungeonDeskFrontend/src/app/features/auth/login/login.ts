import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';

@Component({
  selector: 'app-login',
  imports: [ButtonModule, InputTextModule, FloatLabelModule, PasswordModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {}
