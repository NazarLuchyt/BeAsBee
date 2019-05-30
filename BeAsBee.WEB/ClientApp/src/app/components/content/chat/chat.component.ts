import { Component, OnInit, ViewChild, ElementRef, Output, Input, EventEmitter } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { Chat } from 'src/app/_models/chat.model';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { ChatService } from 'src/app/_services/chat.service';
import { Message } from 'src/app/_models/message.model';
import { Subscription } from 'rxjs';
import { ChatConfigService } from 'src/app/_services/chat-config.service';
import { MessageService } from 'src/app/_services/message.service';
import { encryptMessage } from 'src/app/_helpers/encrypt.helper';

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
  chatConfig: ChatConfigService;

  currentChatId: string;
  inputMessage: string;
  isDisabledChat = false;

  @ViewChild('endList') private endList: ElementRef;
  constructor(private chatService: ChatService, private msgService: MessageService) { }

  ngOnInit() {
    this.chatService.currentChatConfigService.subscribe(cfgService => {
      if (cfgService) {
        this.chatConfig = cfgService;
        this.currentChatId = cfgService.chat.id;
        this.changeChatConfigService(cfgService);
        this.scrollToBottom();
      }
    });
  }

  setColor(userId: string): string {
    return this.colorArray.find(p => p[0] === userId)[1];
  }

  changeChatConfigService(cfgService: ChatConfigService) {
    if (this.storeSubscription) {
      this.storeSubscription.unsubscribe();
    }
    this.storeSubscription = cfgService.messageStore.subscribe(messages => {
      this.messages = messages;
      this.isDisabledChat = cfgService.isChatBlocked;
    });
  }

  scrollToBottom() {
    setTimeout(() => {
      this.endList.nativeElement.scrollTop = this.endList.nativeElement.scrollHeight;
    });
  }

  createMessage(event?: Event): void {
    event.preventDefault();
    if (this.inputMessage) {
      let textMessage = '';
      debugger
      if (this.chatConfig.encryptStatus.value) {
        textMessage = encryptMessage(this.inputMessage, 5);
      }

      const newMessage = new MessageCreate(
        this.chatConfig.chat.id,
        new Date(),
        textMessage.length > 0 ? textMessage : this.inputMessage);
      this.chatConfig.newMessage.next(newMessage);
      this.inputMessage = null;
    }
  }

  load() {
    this.msgService.getPage(this.currentChatId, 1, 3).subscribe((pageResult) => {
      const newMsg = pageResult.items;
      newMsg.push(...this.messages);
      this.chatConfig.changeMessageStore(newMsg);
    });
  }
}
