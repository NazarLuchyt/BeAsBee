import { Injectable, EventEmitter } from '@angular/core';
import { Message } from '../_models/message.model';
import { BehaviorSubject } from 'rxjs';
import { Chat } from '../_models/chat.model';
import { MessageService } from './message.service';
import { MessageCreate } from '../_models/message-create.model';
import { UserPage } from '../_models/user-page.model';

@Injectable()
export class ChatConfigService {

    chat = new Chat();
    isChatBlocked = false;
    // isEncryptActivated = false;
    messageStore: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>(null);
    encryptStatus: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    newMessage: EventEmitter<MessageCreate> = new EventEmitter<MessageCreate>();
    addNewUsers: EventEmitter<UserPage[]> = new EventEmitter<UserPage[]>();
    removeUsers: EventEmitter<string[]> = new EventEmitter<string[]>();
    onChangeEncrytpStatus: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor(private messageService: MessageService) { }

    changeMessageStore(messages: Message[]) {
        this.messageStore.next(messages);
    }

    changeEncryptStatus(status: boolean) {
        this.encryptStatus.next(status);
    }

    // createNewMessage(message: MessageCreate) {
    //     this.messageService.create(message).subscribe(result => {
    //         const newMessageArray = this.store.value;
    //         newMessageArray.push(result);
    //         this.store.next(newMessageArray);
    //     });
    // }
}
