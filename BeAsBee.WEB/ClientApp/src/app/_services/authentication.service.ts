import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Authentication } from '../_models/authentication';
import { UserCreate } from '../_models/user-create.model';
import { map } from 'rxjs/operators';
import * as decode from 'jwt-decode';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AuthenticationService {
  private currentUser: any;
  private jwtHelper: JwtHelperService;

  isLogged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private service: ApiService) {
    this.jwtHelper = new JwtHelperService();
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  changeIsLogged(status: boolean) {
    this.isLogged.next(status);
  }

  isAuthenticated(): boolean {
    if (!this.currentUser) {
      this.isLogged.next(false);
      return false;
    }
    const token = this.currentUser.token;
    const result = !this.jwtHelper.isTokenExpired(token);
    if (!result) {
      localStorage.removeItem('currentUser');
    }
    this.isLogged.next(result);
    return result;
  }


  public register(register: UserCreate) {
    return this.service.post<any>('api/v1/Accounts/registration', register);
    // .subscribe(
    //   result => {
    //     localStorage.setItem('currentUser', result.id);
    //   }
    // );
    // .pipe(map(userd => {
    //   if (user && user.token) {
    //     localStorage.setItem('currentUser', JSON.stringify(user));
    //   }
    //   return user;
    // }));
  }

  public login(authenticate: Authentication) {
    return this.service.post<any>('api/v1/accounts/login', authenticate)
      .pipe(
        map(user => {
          if (user && user.token) {
            localStorage.setItem('currentUser', JSON.stringify(user));
            localStorage.setItem('currentUserGuid', decode(user.token).sub);
            this.currentUser = user;
            this.changeIsLogged(true);
          }
          return user;
        }));
  }

  getUserInfo(id: string) {
    return this.service.getByStringId('api/v1/Users/', id);
  }

  public logout() {
    localStorage.removeItem('currentUserGuid');
    localStorage.removeItem('currentUser');
    this.currentUser = null;
    this.changeIsLogged(false);
  }
}

