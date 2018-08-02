import { environment } from './../../environments/environment';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt';
import { User } from '../_models/user';

@Injectable()
export class AuthService {
    baseUrl = environment.apiUrl + 'auth/';
    userToken: any;
    decodeToken: any;
    jwtHelper: JwtHelper = new JwtHelper();
    currentUser: User;
    photoUrl = new BehaviorSubject<string>('../../assets/user.png');
    currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: Http) { }

 changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

   login(model: any) {
       return this.http.post(this.baseUrl + 'login', model, this.requestOptions())
       .map((response: Response) => {
           const user = response.json();
           if (user && user.tokenstring) {
               localStorage.setItem('token', user.tokenstring);
               localStorage.setItem('user', JSON.stringify(user.user));
               this.currentUser = user.user;
               this.decodeToken = this.jwtHelper.decodeToken(user.tokenstring);
               this.userToken = user.tokenstring;
               this.changeMemberPhoto(this.currentUser.photoUrl);
           }
     }).catch(this.handleError);
   }

   register(model: any) {
       return this.http.post(this.baseUrl + 'register', model, this.requestOptions()).catch(this.handleError);
   }


   loggedIn() {
       return tokenNotExpired();
   }


   private requestOptions() {
       const headers = new Headers({ 'Content-type': 'application/json' });
       return new RequestOptions({headers: headers});
   }

   private handleError(error: any) {
      const applicationError = error.headers.get('Application-Error');
      if (applicationError) {
          return Observable.throw(applicationError);
      }

      const serverError = error.json();
      let modelStateErrors = '';
      if (serverError) {
        for (const key in serverError) {
            if (serverError[key]) {
                modelStateErrors += serverError[key] + '\r\n';
            }

        }
      }
      return Observable.throw(modelStateErrors || 'Server error');
   }
}
