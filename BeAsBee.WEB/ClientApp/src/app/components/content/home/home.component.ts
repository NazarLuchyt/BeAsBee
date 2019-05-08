import { Component, Input, OnInit, Output, EventEmitter, ViewChild, OnDestroy, ViewChildren } from '@angular/core';
import { ChatPreviewComponent } from '../chat-preview/chat-preview.component';
import { ChatComponent } from '../chat/chat.component';
import { ChatHub } from 'src/app/_hubs/chats.hub';
import { UserPage } from 'src/app/_models/user-page.model';
import { Chat } from 'src/app/_models/chat.model';
import { UserService } from 'src/app/_services/user.service';
import { ChatService } from 'src/app/_services/chat.service';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { setChatName, setNameForChats } from 'src/app/_helpers/set-chat-name.helper';
import { SearchComponent } from '../../common/search/search.component';
import { ChatConfigService } from 'src/app/_services/chat-config.service';
import { Message } from 'src/app/_models/message.model';

@Component({
  selector: 'app-home',
  providers: [ChatHub],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit, OnDestroy {
  isSearch: boolean;
  currentUser = new UserPage();
  findedUsers: UserPage[];
  currentChatId: string;
  guidNewChat: string;

  constructor(private userService: UserService, private chatService: ChatService, private chatHub: ChatHub,
    private chatConfig: ChatConfigService) { }

  @Output() transferMessage = new EventEmitter<MessageCreate>();
  @ViewChild(ChatComponent) chatMessage: ChatComponent;
  @ViewChildren(ChatPreviewComponent) chatPreviewArray: ChatPreviewComponent[];
  @ViewChild(SearchComponent) searchComponent: SearchComponent;

  ngOnInit() {

    // --------------
    this.chatHub.started().subscribe(
      sucsses => {
        this.onHubConnected();
      });
    this.chatHub.start();
    // --------------



    this.isSearch = false;
    const id = localStorage.getItem('currentUserGuid');
    const localStorageObject = JSON.parse(localStorage.getItem('currentUser'));
    this.currentUser.firstName = localStorageObject.userFirstName;
    this.currentUser.secondName = localStorageObject.userSecondName;
    this.currentUser.id = id;
    this.chatService.getPage(id, 10, 0, 10).subscribe((result) => {
      this.currentUser.userChats = setNameForChats(result.items, this.currentUser.id);
    });
  }

  onHubConnected() {
    this.chatHub.onChatCreated((chat: Chat) => {
      this.currentUser.userChats.push(setChatName(chat, this.currentUser.id));
    });

    this.chatHub.onUserKicked((chat: Chat, message: string) => {
      const notificationMessage = new Message('-1', message);
      this.chatPreviewArray.find(item => item.chatItem.id === chat.id).pushNotification(chat.id, notificationMessage, true);
      this.scroll();
      // const messages = this.chatConfig.messageStore.value;
      // messages.push(notificationMessage);
      // this.chatConfig.changeMessageStore(messages);
      // this.chatHub.disconnectUserFromChat(chat.id);
    });
  }

  ngOnDestroy(): void {
    this.chatHub.close();
  }

  scroll() {
    this.chatMessage.scrollToBottom();
  }

  getCurrentChatId(chatId: string) {
    this.currentChatId = chatId;
  }

  searchUser(infoToSearch: string) {
    this.userService.getPage(10, 0, infoToSearch).subscribe(
      result => {
        this.findedUsers = result.items;
        this.isSearch = true;
      }
    );
  }

  clearSearch() {
    this.searchComponent.clear();
    this.findedUsers = [];
    this.isSearch = false;
  }

  addNewChatToArray(newChat: Chat) {
    this.guidNewChat = newChat.id;
    this.currentUser.userChats.push(setChatName(newChat, this.currentUser.id));
  }
}
