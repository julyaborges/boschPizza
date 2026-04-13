import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ClienteService } from '../../../core/services/cliente.service';
import { Router } from '@angular/router';
import { Cliente } from '../../../core/models/cliente';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cliente-list',
  imports: [CommonModule],
  templateUrl: './cliente-list.html',
  styleUrl: './cliente-list.css',
})
export class ClienteList {
  
  private clienteService = inject(ClienteService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  clientes: Cliente[] = [];
  loading = true;

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.clienteService.getAll().subscribe({
      next: (response) => {
        this.clientes = response;
      },
      complete: () => {
        this.loading = false;
        this.cdr.markForCheck();
      }
    });
  }

  newCliente(): void {
    this.router.navigate(['/clientes/novo']);
  }

  editCliente(id?: number): void {
    if(!id) return;
    this.router.navigate(['/clientes/editar', id]);
  }

  deleteCliente(id?: number): void {
    if (!id) return;

    const confirmed = window.confirm('Deseja excluir este cliente?');
    if(!confirmed) return;

    this.clienteService.delete(id).subscribe({
      next: () => this.loadData()
    })
  }
}
