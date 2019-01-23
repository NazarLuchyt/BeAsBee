import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ChatHub } from 'src/app/_hubs/chats.hub';
import { defaultImg } from 'src/app/_constants/defaults.const';
import { Chat } from 'src/app/_models/chat.model';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { Message } from 'src/app/_models/message.model';


@Component({
  selector: 'app-chat-preview',
  providers: [ChatHub],
  templateUrl: './chat-preview.component.html',
  styleUrls: ['./chat-preview.component.scss']
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
