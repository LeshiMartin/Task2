import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RegisterModel } from '../../model/registerModel';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {}
  registerModel: RegisterModel = {
    confirmPassword: '',
    password: '',
    userName: '',
  };
  ngOnInit(): void {}

  submit() {
    this.authService.register(this.registerModel).subscribe((x) => {
      this.router.navigate(['/auth']);
    });
  }
}
