import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ChatMembersModalComponent } from '../chat-members-modal/chat-members-modal.component';
import { ChatSettingTypeEnum } from 'src/app/_models/enums/chat-setting-type.enum';
import { ChatService } from 'src/app/_services/chat.service';

@Component({
  selector: 'app-chat-setting',
  templateUrl: './chat-setting.component.html',
  styleUrls: ['./chat-setting.component.scss']
})
export class ChatSettingComponent implements OnInit {

  @Input() chatId: string;
  @ViewChild('chatName') chatName: ElementRef;

  settingType = ChatSettingTypeEnum;
  currentChatName: string;
  currentChatMembersLength: number;


  messageModal: BsModalRef;

  constructor(public baseModalService: BsModalService, private chatService: ChatService) {
  }

  ngOnInit() {
    this.chatService.currentChatConfigService.subscribe(chatCfg => {
      this.currentChatName = chatCfg.chat.name;
      this.currentChatMembersLength = chatCfg.chat.userChats.length;
    });
  }

  showSettingModal(settingType: ChatSettingTypeEnum) {
    this.messageModal = this.baseModalService.show(ChatMembersModalComponent, {
      initialState: {
        chatId: this.chatId,
        settingType: settingType
      }
    });
  }

  dynamicSetWidth() {
    const element: HTMLInputElement = this.chatName.nativeElement;
    const newWidth = (+element.value.length + 8) + 'px';
    element.style.width = newWidth;
    debugger

  }
}
