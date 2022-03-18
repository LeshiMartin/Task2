import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginModel } from '../../model/login-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {}
  loginModel: LoginModel = {
    password: '',
    userName: '',
  };
  ngOnInit(): void {}

  submit() {
    this.authService
      .login(this.loginModel)
      .subscribe((x) => this.router.navigate(['game']));
  }
}
