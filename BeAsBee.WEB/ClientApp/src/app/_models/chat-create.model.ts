import { UserPage } from './user-page.model';
import { Message } from './message.model';

export class ChatCreate {
    constructor(public name: string, public userChats: UserPage[]) { }
}
