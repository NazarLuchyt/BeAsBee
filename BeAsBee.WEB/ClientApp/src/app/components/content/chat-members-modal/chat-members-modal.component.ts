import { Component, OnInit, Input, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { UserService } from 'src/app/_services/user.service';
import { SelectItem } from 'src/app/_models/select-item.model';
import { User } from 'src/app/_models/user.model';
import { ChatService } from 'src/app/_services/chat.service';
import { ChatSettingTypeEnum } from 'src/app/_models/enums/chat-setting-type.enum';
import { settingTypeButtonLable } from 'src/app/_constants/defaults.const';
import { UserPage } from 'src/app/_models/user-page.model';
import { Chat } from 'src/app/_models/chat.model';

@Component({
  selector: 'app-chat-members-modal',
  templateUrl: './chat-members-modal.component.html',
  styleUrls: ['./chat-members-modal.component.scss']
})
export class ChatMembersModalComponent implements OnInit {

  @Input() chatId: string;
  @Input() settingType: ChatSettingTypeEnum;
  @Output() OnClose: Subject<boolean> = new Subject();
  usedMembers: SelectItem[];
  buttonLabel: string;
  public modalRef: BsModalRef;
  currentChat: Chat;

  constructor(public bsModalRef: BsModalRef, private userService: UserService, private chatService: ChatService) {
    this.modalRef = bsModalRef;
  }

  ngOnInit() {
    this.buttonLabel = settingTypeButtonLable[this.settingType];

    this.chatService.currentChatConfigService.subscribe(chatCfg => {
      this.currentChat = chatCfg.chat;
      if (this.settingType === ChatSettingTypeEnum.Remove) {
        this.usedMembers = this.currentChat.userChats.map((item) => ({
          item: item,
          checked: false
        }));
      }
    });


  }

  close() {
    this.modalRef.hide();
    this.OnClose.next(true);
  }

  applyChanges() {
    switch (this.settingType) {
      case ChatSettingTypeEnum.Add: {
        const newUsers = this.usedMembers.filter((item) => item.checked).map(sItem => sItem.item as UserPage);
        this.chatService.currentChatConfigService.value.addNewUsers.emit(newUsers);
        break;
      }
      case ChatSettingTypeEnum.Remove: {
        const userToRemove = this.usedMembers.filter((item) => item.checked).map(sItem => (sItem.item as User).id);
        this.chatService.currentChatConfigService.value.removeUsers.emit(userToRemove);
        break;
      }
    }
  }

  searchUser(infoToSearch: string) {
    switch (this.settingType) {
      case ChatSettingTypeEnum.Add: {
        this.userService.getPage(25, 0, infoToSearch).subscribe(
          result => {
            const difference = result.items.filter(
              findedResult => !this.currentChat.userChats.some(item => item['id'] === findedResult.id)
            );
            this.usedMembers = difference.map((item) => ({
              item: item,
              checked: false
            }));
          }
        );
        break;
      }
      case ChatSettingTypeEnum.Remove: {
        this.usedMembers = this.usedMembers.filter(cMember =>
          ((cMember.item as UserPage).firstName + ' ' + (cMember.item as UserPage).secondName).includes(infoToSearch));
        break;
      }
    }
  }
}
