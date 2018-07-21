import { JwtHelper } from 'angular2-jwt';
import { AuthService } from './services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  jwtHelper: JwtHelper = new JwtHelper();

  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodeToken = this.jwtHelper.decodeToken(token);
    }
  }
}
