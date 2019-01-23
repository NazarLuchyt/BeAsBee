import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Chat } from '../_models/chat.model';
import { Page } from '../_models/page.model';
import { HttpParam } from '../_models/http-param.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private service: ApiService) { }

  public getAll() {
    return this.service.getAll<Array<Chat>>('api/v1/chats');
  }

  public getPage(userId: string, countMessage: number, page: number, countChat: number) {
    const params: HttpParam[] = [
      { key: 'userId', value: `${userId}` },
      { key: 'countMessage', value: `${countMessage}` },
      { key: 'count', value: `${countChat}` },
      { key: 'page', value: `${page}` }];
    return this.service.getPage<Page<Chat>>(`api/v1/chats`, params);
  }

  public getById(id: string) {
    return this.service.getById<Chat>('api/v1/chats/', id);
  }

  public update(user: Chat) {
    return this.service.put<Chat>('api/v1/chats', user);
  }

  public delete(id: string) {
    return this.service.delete<Chat>('api/v1/chats', id);
  }
}
