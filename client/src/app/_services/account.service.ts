import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserJwt } from '../_models/userJwt';
import { BehaviorSubject, ReplaySubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl: string = environment.baseUrl;

  private currentUserSource = new BehaviorSubject<UserJwt | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  register(model: any) {
    return this.http.post<UserJwt>(this.baseUrl + 'account/register', model).pipe(
      map((userJwt: UserJwt) => {
        if (userJwt) {
          this.setCurrentUser(userJwt);
        }
      })
    )
  }

  login(model: any) {
    return this.http.post<UserJwt>(this.baseUrl + 'account/login', model).pipe(
      map((response: UserJwt) => {
        const userJwt = response;
        if (userJwt) {
          localStorage.setItem('user', JSON.stringify(userJwt));
          this.currentUserSource.next(userJwt);
          // this.setCurrentUser(userJwt);
        }
      })
    )
  }


  setCurrentUser(user: UserJwt) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
