import { Injectable } from '@angular/core';
import { ApiService } from './api.services';
import { Message } from '../_models/message.model';
import { MessageCreate } from '../_models/message-create.model';
import { BehaviorSubject } from 'rxjs';
import { MessageStoreService } from './message.store.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private service: ApiService) { }

  public getAll() {
    return this.service.getAll<Array<Message>>('api/v1/messages');
  }

  // public getPage(page: number, count?: number) {
  //   return this.service.getPage<Page<Report>>("api/v1/Reports", page, count);
  // }

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
