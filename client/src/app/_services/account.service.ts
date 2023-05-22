import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserJwt } from '../_models/userJwt';
import { BehaviorSubject, Observable, ReplaySubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl: string = environment.baseUrl;

  private currentUserSource = new BehaviorSubject<UserJwt | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,
    private router: Router) { }

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
          // localStorage.setItem('user', JSON.stringify(userJwt));
          // this.currentUserSource.next(userJwt);
          this.setCurrentUser(userJwt);
        }
      })
    )
  }

  LoginWithGoogle(credentials: string): Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post(this.baseUrl + "account/google-login", JSON.stringify(credentials), { headers: header, withCredentials: true });
  }


  setCurrentUser(user: UserJwt) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
    window.location.reload();
  }
}
