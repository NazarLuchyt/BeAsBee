import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Authentication } from '../_models/authentication';
import { UserCreate } from '../_models/user-create.model';

@Injectable()
export class AuthenticationService {

  constructor(private service: ApiService) { }

  public register(register: UserCreate) {
    return this.service.post<any>('api/v1/Account/registration', register);
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
    const result = this.service.post<any>('api/v1/Account/login', authenticate);
    return result;
  }

  getUserInfo(id: string) {
    return this.service.getByStringId('api/v1/Users/', id)
  }
  public logout() {
    localStorage.removeItem('currentUserId');
  }
}

