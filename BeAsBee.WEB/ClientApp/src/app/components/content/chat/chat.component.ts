import { Component, OnInit, ViewChild, ElementRef, Output, Input, EventEmitter } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { defaultImg } from 'src/app/_constants/defaults.const';
import { UserPage } from 'src/app/_models/user-page.model';
import { Chat } from 'src/app/_models/chat.model';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { ChatService } from 'src/app/_services/chat.service';
import { Message } from 'src/app/_models/message.model';
import { MessageStoreService } from 'src/app/_services/message.store.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})

export class ChatComponent implements OnInit {
  colorArray = [];
  defaultColor: string[] = ['#c44242', '#5119c5', '#0f9497', '#3b7919'];
  hubConnection: HubConnection;
  messages: Message[];

  storeSubscription: Subscription;
  messageStore: MessageStoreService;

  chatId: string;
  inputMessage: string;
  currentChat: Chat;

  @ViewChild('endList') private endList: ElementRef;
  @ViewChild('input_field') private inputField: ElementRef;

  constructor(private chatService: ChatService) { }

  ngOnInit() {
    this.chatService.currentMessageStore.subscribe(msgStore => {
      if (msgStore) {
        this.messageStore = msgStore;
        this.changeMessageStore(msgStore);
        this.scrollToBottom();
      }
    });
  }

  setColor(userId: string): string {
    return this.colorArray.find(p => p[0] === userId)[1];
  }

  changeMessageStore(msgStore: MessageStoreService) {
    if (this.storeSubscription) {
      this.storeSubscription.unsubscribe();
    }
    this.storeSubscription = msgStore.store.subscribe(messages => {
      this.messages = messages;
    });
  }

  scrollToBottom() {
    setTimeout(() => {
      this.endList.nativeElement.scrollTop = this.endList.nativeElement.scrollHeight;
    });
  }

  public createMessage(event: Event): void {
    if (this.inputMessage) {
      const newMessage = new MessageCreate(
        this.messageStore.chat.id,
        new Date(),
        this.inputMessage);
      this.messageStore.newMessage.next(newMessage);
      this.inputMessage = null;
    }
    event.preventDefault();
  }
}
