
import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { User } from '../_models/user.model';
import { UserPage } from '../_models/user-page.model';
import { UserViewTypeEnum } from '../_models/enums/user-view-type.enum';
import { HttpParam } from '../_models/http-param.model';
import { Page } from '../_models/page.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private service: ApiService) { }

  public getAll() {
    return this.service.getAll<Array<User>>('api/v1/users');
  }

  public getPage(count: number, page: number, infoToSearch: string) {
    const params: HttpParam[] = [
      { key: 'infoToSearch', value: `${infoToSearch}` },
      { key: 'count', value: `${count}` },
      { key: 'page', value: `${page}` }];
    return this.service.getPage<Page<UserPage>>(`api/v1/users`, params);
  }

  public getById<T>(id: string, userViewType: UserViewTypeEnum) {
    if (userViewType === UserViewTypeEnum.Home) {
      return this.service.getById<T>('api/v1/users/', id + ',' + userViewType);
    }
    if (userViewType === UserViewTypeEnum.View) {
      return this.service.getById<T>('api/v1/users/', id + ',' + userViewType);
    }
  }

  public register(user: User) {
    return this.service.post<User>('api/v1/users/register', user);
  }

  public update(user: User) {
    return this.service.put<User>('api/v1/users', user);
  }

  public delete(id: string) {
    return this.service.delete<User>('api/v1/users', id);
  }
}
