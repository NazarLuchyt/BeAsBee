import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Chat } from '../_models/chat.model';
import { defaultImg } from '../_constants/defaults.const';
import { ChatHub } from '../_hubs/chats.hub';
import { Message } from '../_models/message.model';
import { MessageCreate } from '../_models/message-create.model';

@Component({
  selector: 'app-chat-preview',
  providers: [ChatHub],
  templateUrl: './chat-preview.component.html',
  styleUrls: ['./chat-preview.component.css']
})

export class ChatPreviewComponent implements OnInit, OnDestroy {
  defaultImg = defaultImg;

  @Output() selectedChat = new EventEmitter<Chat>();
  @Output() pushMessage = new EventEmitter();

  @Input() chat: Chat;

  constructor(private chatHub: ChatHub) { }

  ngOnInit() {
    this.chatHub.started().subscribe(
      sucsses => {
        this.onHubConnected();
      });
    this.chatHub.start();
  }

  onHubConnected() {
    this.chatHub.onSend((connectionId: string, message: Message) => {
      this.chat.messages.push(message);
      this.pushMessage.next();
    });

    this.chatHub.connectToChat(this.chat.id).subscribe();
  }

  ngOnDestroy(): void {
    this.chatHub.close();
  }

  setChat(chat: Chat) {
    this.selectedChat.next(chat);
  }

  sentMessage(message: MessageCreate) {
    this.chatHub.sendMessage(message);
  }
}
