import { UserPage } from './user-page.model';
import { Message } from './message.model';


export class Chat {
    id: string;
    name: string;
    userId: string;
    messages: Message[];
    userChats: UserPage[];

    // constructor(id: string, name: string, userId: string, messages: Message[], userChats: UserPage[]) {
    //     this.id = id;
    //     this.name = name;
    //     this.userId = userId;
    //     this.messages = messages;
    //     this.userChats = userChats;
    // }
}
