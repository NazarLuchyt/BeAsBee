import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserPage } from 'src/app/_models/user-page.model';
import { defaultImg } from 'src/app/_constants/defaults.const';
import { ChatCreate } from 'src/app/_models/chat-create.model';
import { ChatService } from 'src/app/_services/chat.service';
import { Chat } from 'src/app/_models/chat.model';


@Component({
  selector: 'app-user-preview',
  templateUrl: './user-preview.component.html',
  styleUrls: ['./user-preview.component.scss']
})
export class UserPreviewComponent implements OnInit {
  defaultImg = defaultImg;
  findedUsers: UserPage[];

  @Input()
  set setfindedUsers(users: UserPage[]) {
    this.findedUsers = users;
  }
  get setfindedUsers() { return this.findedUsers; }

  @Output() returnNewChat = new EventEmitter<Chat>();

  constructor(private chatService: ChatService) { }

  ngOnInit() {
  }

  createNewChat(selectedUser: UserPage) {
    if (window.confirm('Create chat?')) {
      const chatsUsers: UserPage[] = [];
      chatsUsers.push(selectedUser);
      const newChat: ChatCreate = new ChatCreate('', chatsUsers);
      this.chatService.create(newChat).subscribe(result => {
        
        this.returnNewChat.next(result);
      });
    }
  }

}
