import { AlertifyService } from './../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '../../../node_modules/@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  login_name = '';
   photoUrl: string;

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
     this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(data => {
      this.alertify.success('logged in sucessfully ' + this.authService.decodeToken.unique_name);
      this.login_name = this.authService.decodeToken.unique_name;
    }, error => {
      this.alertify.error('login failed' + error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  logout() {
    this.authService.userToken = null;
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.authService.decodeToken = null;
    this.alertify.message('logout');
    this.router.navigate(['/home']); 
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

}
