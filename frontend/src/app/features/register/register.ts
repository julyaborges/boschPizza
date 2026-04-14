import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

  private fb = inject(FormBuilder);
  private authService = inject(Auth);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  loading = false;
  errorMessage = '';
  successMessage = '';

  form = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  submit(): void {

    if (this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const { username, password } = this.form.value;

    this.authService.register({ username, password }).subscribe({

      next: () => {
        this.successMessage = 'Usuário cadastrado com sucesso!';

        // redireciona depois de 2 segundos
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },

      error: (err) => {
        if (err.status === 400) {
          this.errorMessage = err.error?.message || 'Erro ao cadastrar usuário';
        } else {
          this.errorMessage = 'Erro ao conectar com o servidor';
        }

        this.loading = false;
        this.cdr.markForCheck();
      },

      complete: () => {
        this.loading = false;
        this.cdr.markForCheck();
      }

    });
  }
}