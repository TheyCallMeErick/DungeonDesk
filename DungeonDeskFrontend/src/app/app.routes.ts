import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'desks',
    pathMatch: 'full',
  },
  {
    path: 'auth',
    loadComponent: () => import('./layouts/auth/auth').then((m) => m.Auth),
    children: [
      {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
        //outlet: 'auth',
      },
      {
        path: 'register',
        loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
        //outlet: 'auth',
      },
      {
        path: 'forgot-password',
        loadComponent: () =>
          import('./features/auth/forgot-password/forgot-password').then((m) => m.ForgotPassword),
        //outlet: 'auth',
      },
    ],
  },
];
