import { AlertifyService } from './../services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(data => {
      this.alertify.success('logged in sucessfully')
    }, error => {
      this.alertify.error('login failed' + error);
    });
  }

  logout() {
    this.authService.userToken = null;
    localStorage.removeItem('token');
    this.alertify.message('logout');
  }

  loggedin() {
    return this.authService.loggedIn();
  }

}
