import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { IToken } from '../shared/models/token';
import { IUser } from '../shared/models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((token: IToken) => {
        if (token) {
          localStorage.setItem('token', token.token);
          this.getAccount();
        }
      })
    )
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((token: IToken) => {
        if (token) {
          localStorage.setItem('token', token.token);
          this.getAccount();
        }
      })
    )
  }

  getAccount() {
    return this.http.get(this.baseUrl + 'account').pipe(
      map((user: IUser) => {
        if (user) {
          this.currentUserSource.next(user);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

}
