import { User } from '../_models/user';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable()
export class UserService {
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.baseUrl + 'users');
    }

    getUser(id): Observable<User> {
        return this.http.get<User>(this.baseUrl + 'users/' + id);
    }

    updateUser(id, user: User): Observable<User> {
        return this.http.put<User>(this.baseUrl + 'users/' + id, user);
    }

    setMainPhoto(userId: number, id: number) {
        return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
    }

}
