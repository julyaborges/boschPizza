import { Routes } from '@angular/router';
import { AccessDenied } from './features/auth/pages/access-denied/access-denied';
import { MainLayout } from './layout/components/main-layout/main-layout';
import { authGuard } from './core/guards/auth-guard';
import { PizzaForm } from './features/pizzas/pages/pizza-form/pizza-form';
import { Login } from './features/auth/pages/login/login';
import { Home } from './features/dashboard/pages/home/home';
import { PizzaList } from './features/pizzas/pages/pizza-list/pizza-list';
import { ClienteList } from './features/clientes/cliente-list/cliente-list';
import { ClienteForm } from './features/clientes/cliente-form/cliente-form';
import { Register } from './features/register/register';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' }, // se for vazio redireciona para login
    { path: 'login', component: Login}, // 
    { path: 'acesso-negado', component: AccessDenied},
    { path: 'register', component: Register},
    {
        path: '',
        component: MainLayout,
        canActivate: [authGuard],
        children: [
            { path: 'home', component: Home},
            { path: 'pizzas', component: PizzaList},
            { path: 'pizzas/novo', component: PizzaForm},
            { path: 'pizzas/editar/:id', component: PizzaForm},
            { path: 'clientes', component: ClienteList},
            { path: 'clientes/novo', component: ClienteForm},
            { path: 'clientes/editar/:id', component: ClienteForm}
        ]
    },
    { path: '**', redirectTo: 'acesso-negado'}
];
