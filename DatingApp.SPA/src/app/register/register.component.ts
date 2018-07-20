import { AlertifyService } from './../services/alertify.service';
import { AuthService } from './../services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter();

  constructor(private authServices: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authServices.register(this.model).subscribe(() => {
      this.alertify.success('Registration ok');
    }, error => {
      this.alertify.error('registration failed' + error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.message('cancel');
  }

}
