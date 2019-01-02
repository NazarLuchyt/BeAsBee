import { Injectable } from '@angular/core';
import { BaseHub } from '../_hubs/base.hub';
import { Message } from '../_models/message.model';
import { User } from '../_models/user.model';
import { MessageCreate } from '../_models/message-create.model';
import { Observable, from } from 'rxjs';

@Injectable()

export class ChatHub extends BaseHub {

  constructor() {
    super('chat');
  }

  onNewUserAdded(fn: (connectionId: string, error: string) => void) {
    this.hubConnection.on('onNewUserAdded', fn);
  }

  onDisconnected(fn: (connectionId: string, user: User, error: string) => void) {
    this.hubConnection.on('OnDisconnected', fn);
  }

  onConnected(fn: (connectionId: string, user: User, message: Message) => void) {
    this.hubConnection.on('OnConnected', fn);
  }

  onSend(fn: (connectionId: string, message: Message) => void) {
    this.hubConnection.on('OnSend', fn);
  }

  onConnectToChat(fn: (connectionId: string, user: User, message: Message) => void) {
    this.hubConnection.on('OnConnectToChat', fn);
  }

  onUserStatusChange(fn: (connectionId: string, userId: string) => void) {
    this.hubConnection.on('OnUserStatusChange', fn);
  }

  sendMessage(message: MessageCreate): Observable<Message> {
    return from(this.hubConnection.invoke('SendAsync', message));
  }

  connectToChat(chatId: string) {
    return from(this.hubConnection.invoke('ConnectToChatAsync', chatId));
  }

  changeUserStatus(status, chatId) {
    return from(this.hubConnection.invoke('UserStatusChangeAsync', status, chatId));
  }

  started(): Observable<any> {
    return this.starting;
  }

  closed(): Observable<any> {
    return this.closing;
  }
}
