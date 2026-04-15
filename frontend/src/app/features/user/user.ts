import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../core/services/auth';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User implements OnInit {

  private authService = inject(Auth);
  private cdr = inject(ChangeDetectorRef);

  user: any[] = [];
  loading = false;

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    this.loading = true;

    this.authService.getUsers().subscribe({
      next: (data) => {
        this.user = data;
      },
      complete: () => {
        this.loading = false;
        this.cdr.markForCheck();
      }
    });
  }

  deleteUser(id: number) {
    if (!confirm('Deseja realmente excluir este usuário?')) return;

    this.authService.deleteUser(id).subscribe({
      next: () => {
        this.loadUser(); 
      }
    });
  }
}