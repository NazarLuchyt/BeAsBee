import { Component, OnInit, Input } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ChatMembersModalComponent } from '../chat-members-modal/chat-members-modal.component';
import { ChatSettingTypeEnum } from 'src/app/_models/enums/chat-setting-type.enum';

@Component({
  selector: 'app-chat-setting',
  templateUrl: './chat-setting.component.html',
  styleUrls: ['./chat-setting.component.scss']
})
export class ChatSettingComponent implements OnInit {

  @Input() chatId: string;

  settingType = ChatSettingTypeEnum;

  messageModal: BsModalRef;

  constructor(public baseModalService: BsModalService) {
  }

  ngOnInit() {
  }

  showSettingModal(settingType: ChatSettingTypeEnum) {
    this.messageModal = this.baseModalService.show(ChatMembersModalComponent, {
      initialState: {
        chatId: this.chatId,
        settingType: settingType
      }
    });
  }

}
