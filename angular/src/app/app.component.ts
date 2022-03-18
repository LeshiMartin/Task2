import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private authService: AuthService, private router: Router) {}
  isLoggedIn$ = this.authService.userLoggedInStatus$;
  logOut() {
    this.authService.logout();
    this.router.navigate(['auth']);
  }
  title = 'angular';
}
