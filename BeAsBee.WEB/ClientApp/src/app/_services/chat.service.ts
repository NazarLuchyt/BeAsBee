import { Injectable, EventEmitter } from '@angular/core';
import { ApiService } from './api.services';
import { Chat } from '../_models/chat.model';
import { Page } from '../_models/page.model';
import { HttpParam } from '../_models/http-param.model';
import { BehaviorSubject } from 'rxjs';
import { ChatCreate } from '../_models/chat-create.model';
import { User } from '../_models/user.model';
import { ChatConfigService } from './chat-config.service';

@Injectable()
export class ChatService {

  currentChatConfigService: BehaviorSubject<ChatConfigService> = new BehaviorSubject<ChatConfigService>(null);

  constructor(private service: ApiService) { }

  changeChatConfigService(configInstance: ChatConfigService) {
    this.currentChatConfigService.next(configInstance);
  }

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

  public create(model: ChatCreate) {
    return this.service.post<any>('api/v1/chats', model);
  }

  public update(user: Chat) {
    return this.service.put<Chat>('api/v1/chats', user);
  }

  public addUsers(chatId: string, userGuids: string[]) {
    return this.service.post<string[]>('api/v1/chats/addUsers/' + chatId, userGuids);
  }

  public delete(id: string) {
    return this.service.delete<Chat>('api/v1/chats', id);
  }
}
