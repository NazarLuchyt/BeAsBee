import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Message } from '../_models/message.model';
import { MessageCreate } from '../_models/message-create.model';
import { HttpParam } from '../_models/http-param.model';
import { Page } from '../_models/page.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private service: ApiService) { }

  public getAll() {
    return this.service.getAll<Array<Message>>('api/v1/messages');
  }

  public getPage(chatId: string, page: number, count?: number) {
    const params: HttpParam[] = [
      { key: 'chatId', value: `${chatId}` },
      { key: 'count', value: `${count}` },
      { key: 'page', value: `${page}` }];
    return this.service.getPage<Page<Message>>('api/v1/Messages', params);

  }

  //{{localUrl}}/api/v1/Messages?chatId=29f8aace-cbc7-47dc-a16c-c6b83b1afc38&count=3&page=2

  public getByChatId(id: string) {
    return this.service.getById<Array<Message>>('api/v1/messages/', id);
  }

  public create(model: MessageCreate) {
    return this.service.post<any>('api/v1/messages', model);
  }

  public update(message: Message) {
    return this.service.put<Message>('api/v1/messages', message);
  }

  public delete(id: string) {
    return this.service.delete<Message>('api/v1/messages', id);
  }
}
