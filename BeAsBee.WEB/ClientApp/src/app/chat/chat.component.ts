import { Input, Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Message } from '../_models/message.model';
import { defaultImg } from '../_constants/defaults.const';
import { UserPage } from '../_models/user-page.model';
import { MessageCreate } from '../_models/message-create.model';
import { HubConnection } from '@aspnet/signalr';
import { ChatService } from '../_services/chat.service';
import { Chat } from '../_models/chat.model';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})

export class ChatComponent implements OnInit {
  colorArray = [];
  defaultColor: string[] = ['#c44242', '#5119c5', '#0f9497', '#3b7919'];
  hubConnection: HubConnection;
  messages: Message[];
  defaultImg = defaultImg;
  chatId: string;
  user: UserPage;
  inputMessage: string;
  currentChat: Chat;

  @ViewChild('endList') private endList: ElementRef;

  @Output() createdMessage = new EventEmitter<MessageCreate>();

  @Input()
  set setMessages(messages: Message[]) {
    this.messages = messages;
  }
  get setMessages() { return this.messages; }

  @Input()
  set setChatId(id: string) {
    this.chatId = id;
    this.chatService.getById(this.chatId).subscribe(result => {
      this.currentChat = result;
      let i = 0;
      this.currentChat.userChats.forEach(user => {
        this.colorArray[i] = [user.id, this.defaultColor[i]];
        i++;
      });
    });
  }

  setColor(userId: string): string {
    return this.colorArray.find(p => p[0] === userId)[1];
  }

  get setChatId() { return this.chatId; }

  @Input()
  set setUser(user: UserPage) {
    this.user = user;
    if (user) {
    }
  }
  get setUser() { return this.user; }

  constructor(private chatService: ChatService) { }

  ngOnInit() {
  }

  scrollToBottom() {
    setTimeout(() => {
      this.endList.nativeElement.scrollTop = this.endList.nativeElement.scrollHeight;
    });
  }

  public createMessage(): void {
    const newMessage = new MessageCreate(
      this.chatId,
      new Date(),
      this.inputMessage,
      this.user.id,
      this.user.firstName + ' ' + this.user.secondName);

    this.createdMessage.emit(newMessage); // send message to parent component

    this.inputMessage = '';
  }
}
