import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClienteService } from '../../../core/services/cliente.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cliente-form',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './cliente-form.html',
  styleUrl: './cliente-form.css',
})
export class ClienteForm implements OnInit {

  private fb = inject(FormBuilder);
  private clienteService = inject(ClienteService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id?: number;
  loading = false;
  
  form = this.fb.group({
    id: [0],
    name: ['', [Validators.required, Validators.minLength(3)]],
    address: ['', [Validators.required, Validators.minLength(5)]],
    phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,11}$/)]]
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if(idParam){
      this.id = Number(idParam);
      this.loadCliente(this.id);
    }
  }

  loadCliente(id: number): void{
    this.loading = true;

    this.clienteService.getById(id).subscribe({
      next: (cliente) => {
        this.form.patchValue(cliente);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  save(): void {
    //  Validação
    if (this.form.invalid) {
      alert('Obrigatório preencher os campos!');
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.form.getRawValue();
    
    if(this.id){
      this.clienteService.update(this.id, payload).subscribe({
        next: () => this.router.navigate(['/clientes'])
      });
      return;
    }

    this.clienteService.create(payload).subscribe({
      next: () => this.router.navigate(['/clientes'])
    })
  }
}
