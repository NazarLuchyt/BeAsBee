import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ChatHub } from 'src/app/_hubs/chats.hub';
import { defaultImg } from 'src/app/_constants/defaults.const';
import { Chat } from 'src/app/_models/chat.model';
import { Message } from 'src/app/_models/message.model';
import { ChatService } from 'src/app/_services/chat.service';
import { ChatConfigService } from 'src/app/_services/chat-config.service';
import { UserPage } from 'src/app/_models/user-page.model';
import { ChatSettingTypeEnum } from 'src/app/_models/enums/chat-setting-type.enum';

@Component({
  selector: 'app-chat-preview',
  providers: [ChatHub, ChatConfigService],
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

  constructor(private chatHub: ChatHub, private chatService: ChatService, public chatConfig: ChatConfigService) {
    this.chatConfig.removeUsers.subscribe((users: string[]) => {
      this.chatHub.removeUsers(this.chatItem.id, users).subscribe(() => {
        users.forEach(userGuid => {
          const index = this.chatConfig.chat.userChats.findIndex(user => user.id === userGuid);
          if (index > -1) {
            this.chatConfig.chat.userChats.splice(index, 1);
          }
        });
      },
        error => { }
      );
    });

    this.chatConfig.addNewUsers.subscribe((users: UserPage[]) => {
      const newUserGuids = users.map(user => user.id);
      // const notificationMessage = new Message('-1', "TETEST ADADADAD"); // TODO add logic
      // this.pushNotification(this.chatItem.id, notificationMessage);
      this.chatHub.startNewChat(this.chatItem.id, newUserGuids).subscribe(() => {
        this.chatConfig.chat.userChats.push(...users);
      },
        error => { }
      );
    });
  }

  ngOnInit() {
    this.chatConfig.changeMessageStore(this.chatItem.messages);
    this.chatConfig.chat = this.chatItem;
    this.chatConfig.messageStore.subscribe(result => {
      this.lastMessage = result[result.length - 1];
    });

    this.chatHub.started().subscribe(
      sucsses => {
        this.onHubConnected();
      });
    this.chatHub.start();
    this.chatConfig.newMessage.subscribe(newMessage => {
      if (this.chatHub) {
        this.chatHub.sendMessage(newMessage);
      }
    });
  }

  onHubConnected() {
    this.chatHub.onSend((connectionId: string, message: Message) => {
      const newMessageArray = this.chatConfig.messageStore.value;
      newMessageArray.push(message);
      this.chatConfig.messageStore.next(newMessageArray);
      this.pushMessage.next();
    });

    this.chatHub.connectToChat(this.chatItem.id).subscribe();
    if (this.isNewChat) {
      this.chatHub.createNewChat(this.chatItem.id);
      this.isNewChat = false;
    }

    this.chatHub.onRemoveUsers((chat: Chat, message: string) => {
      const notificationMessage = new Message('-1', message);
      this.pushNotification(chat.id, notificationMessage);
    });
  }

  pushNotification(chatGuid: string, message: Message, isKicked?: boolean) {
    const messages = this.chatConfig.messageStore.value;
    messages.push(message);
    if (isKicked) {
      this.chatConfig.isChatBlocked = isKicked;
    }
    this.chatConfig.changeMessageStore(messages);
  }

  ngOnDestroy(): void {
    this.chatHub.close();
  }

  getChat() {
    this.returnChatId.next(this.chatItem.id);
    this.chatService.changeChatConfigService(this.chatConfig);
  }

}
