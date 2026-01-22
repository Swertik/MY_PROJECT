import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Нужно для ngModel
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [FormsModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class Auth {
  username = signal('');
  password = signal('');

  constructor(private router: Router) {}

  onLogin() {
    const user = this.username();
    const pass = this.password();

    if (user && pass) {
      console.log('Login data:', { user, pass });
      this.router.navigate(['/dashboard']);
    }
  }
}
