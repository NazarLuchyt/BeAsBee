import { Injectable, EventEmitter } from '@angular/core';
import { Message } from '../_models/message.model';
import { BehaviorSubject } from 'rxjs';
import { Chat } from '../_models/chat.model';
import { MessageService } from './message.service';
import { MessageCreate } from '../_models/message-create.model';

@Injectable()
export class MessageStoreService {

    chat = new Chat();
    store: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>(null);
    newMessage: EventEmitter<MessageCreate> = new EventEmitter<MessageCreate>();

    constructor(private messageService: MessageService) { }

    changeMessageStore(messages: Message[]) {
        this.store.next(messages);
    }

    // createNewMessage(message: MessageCreate) {
    //     this.messageService.create(message).subscribe(result => {
    //         const newMessageArray = this.store.value;
    //         newMessageArray.push(result);
    //         this.store.next(newMessageArray);
    //     });
    // }
}
