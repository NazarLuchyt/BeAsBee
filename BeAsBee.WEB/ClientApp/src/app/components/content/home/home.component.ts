import { Component, Input, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { ChatPreviewComponent } from '../chat-preview/chat-preview.component';
import { ChatComponent } from '../chat/chat.component';
import { ChatHub } from 'src/app/_hubs/chats.hub';
import { UserPage } from 'src/app/_models/user-page.model';
import { Chat } from 'src/app/_models/chat.model';
import { UserService } from 'src/app/_services/user.service';
import { ChatService } from 'src/app/_services/chat.service';
import { MessageCreate } from 'src/app/_models/message-create.model';
import { UserViewTypeEnum } from 'src/app/_models/enums/user-view-type.enum';
import { Message } from 'src/app/_models/message.model';

@Component({
  selector: 'app-home',
  providers: [ChatHub],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
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
