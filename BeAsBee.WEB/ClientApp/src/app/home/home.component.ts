import { Component, Input, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { UserService } from '../_services/user.service';
import { UserViewTypeEnum } from '../_models/enums/user-view-type.enum';
import { UserPage } from '../_models/user-page.model';
import { ChatService } from '../_services/chat.service';
import { Chat } from '../_models/chat.model';
import { Message } from '../_models/message.model';
import { ChatHub } from '../_hubs/chats.hub';
import { MessageCreate } from '../_models/message-create.model';
import { ChatPreviewComponent } from '../chat-preview/chat-preview.component';
import { ChatComponent } from '../chat/chat.component';

@Component({
  selector: 'app-home',
  providers: [ChatHub],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  isSearch: boolean;
  currentUser: UserPage;
  findedUsers: UserPage[];
  usersChats: Chat[];
  currentChatId: string;
  currentMessages: Message[];
  constructor(private userService: UserService, private chatService: ChatService, private chatHub: ChatHub) { }

  @Output() transferMessage = new EventEmitter<MessageCreate>();

  @ViewChild(ChatComponent) chatMessage: ChatComponent;
  @ViewChild(ChatPreviewComponent) chatPreview: ChatPreviewComponent;

  ngOnInit() {
    this.isSearch = false;
    const id = localStorage.getItem('currentUserId');

    this.chatService.getPage(id, 5, 0, 10).subscribe((result) => {
      this.usersChats = result.items;
      this.setChats(id, this.usersChats);
    });
  }

  setChats(id: string, chats: Chat[]) {

    this.userService.getById<UserPage>(id, UserViewTypeEnum.Home)
      .subscribe(result => {
        this.currentUser = result;
        this.currentUser.usersChats = this.usersChats;
      });
  }

  setChat(chat: Chat) {
    this.currentMessages = chat.messages;
    this.currentChatId = chat.id;
  }

  scroll() {
    this.chatMessage.scrollToBottom();
  }

  getMessage(message: MessageCreate) {
    this.chatPreview.sentMessage(message);
  }

  searchUser(infoToSearch: string) {
    this.userService.getPage(20, 0, infoToSearch).subscribe(
      result => {
        this.findedUsers = result.items;
        this.isSearch = true;
      }
    );
  }
}
