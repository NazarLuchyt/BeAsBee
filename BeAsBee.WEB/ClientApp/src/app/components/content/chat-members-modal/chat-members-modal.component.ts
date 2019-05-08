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

@Component({
  selector: 'app-chat-members-modal',
  templateUrl: './chat-members-modal.component.html',
  styleUrls: ['./chat-members-modal.component.scss']
})
export class ChatMembersModalComponent implements OnInit {

  @Input() chatId: string;
  @Input() settingType: ChatSettingTypeEnum;
  @Output() OnClose: Subject<boolean> = new Subject();
  chatMembers: SelectItem[];
  buttonLabel: string;
  public modalRef: BsModalRef;

  constructor(public bsModalRef: BsModalRef, private userService: UserService, private chatService: ChatService) {
    this.modalRef = bsModalRef;
  }

  ngOnInit() {
    this.buttonLabel = settingTypeButtonLable[this.settingType];
    if (this.settingType === ChatSettingTypeEnum.Remove) {
      this.chatMembers = this.chatService.currentChatConfigService.value.chat.userChats.map((item) => ({
        item: item,
        checked: false
      }));
    }
  }

  close() {
    this.modalRef.hide();
    this.OnClose.next(true);
  }

  applyChanges() {
    switch (this.settingType) {
      case ChatSettingTypeEnum.Add: {
        const newUsers = this.chatMembers.filter((item) => item.checked).map(sItem => sItem.item as UserPage);
        this.chatService.currentChatConfigService.value.addNewUsers.emit(newUsers);
        break;
      }
      case ChatSettingTypeEnum.Remove: {
        const userToRemove = this.chatMembers.filter((item) => item.checked).map(sItem => (sItem.item as User).id);
        this.chatService.currentChatConfigService.value.removeUsers.emit(userToRemove);
        break;
      }
    }
    // debugger
    // this.chatService.addUsers(this.chatId, newUserGuids).subscribe(result => {
    //   const test = result;
    //   debugger
    // });
  }

  searchUser(infoToSearch: string) {
    switch (this.settingType) {
      case ChatSettingTypeEnum.Add: {
        this.userService.getPage(10, 0, infoToSearch).subscribe(
          result => {
            this.chatMembers = result.items.map((item) => ({
              item: item,
              checked: false
            }));
          }
        );
        break;
      }
      case ChatSettingTypeEnum.Remove: {
     //   const t = ('Павло' + ' ' + 'Дідик').includes(infoToSearch);


        this.chatMembers = this.chatMembers.filter(cMember =>
          ((cMember.item as UserPage).firstName + ' ' + (cMember.item as UserPage).secondName).includes(infoToSearch));
        break;
      }
    }
  }
}
