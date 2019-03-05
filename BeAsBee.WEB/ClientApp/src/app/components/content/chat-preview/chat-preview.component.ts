import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ChatHub } from 'src/app/_hubs/chats.hub';
import { defaultImg } from 'src/app/_constants/defaults.const';
import { Chat } from 'src/app/_models/chat.model';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { Message } from 'src/app/_models/message.model';
import { MessageStoreService } from 'src/app/_services/message.store.service';
import { ChatService } from 'src/app/_services/chat.service';

@Component({
  selector: 'app-chat-preview',
  providers: [ChatHub, MessageStoreService],
  templateUrl: './chat-preview.component.html',
  styleUrls: ['./chat-preview.component.scss']
})

export class ChatPreviewComponent implements OnInit, OnDestroy {
  defaultImg = defaultImg;
  lastMessage: Message;


  @Input() chatItem: Chat;
  @Input() isNewChat: false;
  @Output() pushMessage = new EventEmitter();
  @Output() returnChatId = new EventEmitter();

  constructor(private chatHub: ChatHub, private chatService: ChatService, public messageStore: MessageStoreService) { }

  ngOnInit() {
    this.messageStore.changeMessageStore(this.chatItem.messages);
    this.messageStore.chat = this.chatItem;
    this.messageStore.store.subscribe(result => {
      this.lastMessage = result[result.length - 1];
    });

    this.chatHub.started().subscribe(
      sucsses => {
        this.onHubConnected();
      });
    this.chatHub.start();
    this.messageStore.newMessage.subscribe(newMessage => {
      if (this.chatHub) {
        this.chatHub.sendMessage(newMessage);
      }
    });
  }

  onHubConnected() {
    this.chatHub.onSend((connectionId: string, message: Message) => {
      const newMessageArray = this.messageStore.store.value;
      newMessageArray.push(message);
      this.messageStore.store.next(newMessageArray);
      this.pushMessage.next();
    });
    this.chatHub.connectToChat(this.chatItem.id).subscribe();
    if (this.isNewChat) {
      this.chatHub.createNewChat(this.chatItem.id);
      this.isNewChat = false;
    }
  }

  ngOnDestroy(): void {
    this.chatHub.close();
  }

  getChat() {
    this.returnChatId.next(this.chatItem.id);
    this.chatService.changeCurrentMessageStore(this.messageStore);
  }

}
