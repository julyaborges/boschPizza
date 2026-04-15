import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginResponse } from '../models/login-response';
import { LoginRequest } from '../models/login';
import { Observable } from 'rxjs';
 
@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'http://localhost:5157/auth';
 
  login(data: LoginRequest): Observable<LoginResponse>{
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data);
  }

  register(data: LoginRequest): Observable<any>{
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/users`);
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));

    return (
      payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
      payload["role"] ||
      null
    );
   

  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${id}`);
  }
 
  saveToken(token: string): void{
    localStorage.setItem('token', token);
  }
 
  getToken(): string | null {
    return localStorage.getItem('token');
  }
 
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
 
  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  
}