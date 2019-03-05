import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Chat } from '../_models/chat.model';
import { Page } from '../_models/page.model';
import { HttpParam } from '../_models/http-param.model';
import { BehaviorSubject } from 'rxjs';
import { MessageStoreService } from './message.store.service';
import { ChatCreate } from '../_models/chat-create.model';

@Injectable()
export class ChatService {

  currentMessageStore: BehaviorSubject<MessageStoreService> = new BehaviorSubject<MessageStoreService>(null);

  constructor(private service: ApiService) { }

  changeCurrentMessageStore(storeInstance: MessageStoreService) {
    this.currentMessageStore.next(storeInstance);
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

  public delete(id: string) {
    return this.service.delete<Chat>('api/v1/chats', id);
  }
}
