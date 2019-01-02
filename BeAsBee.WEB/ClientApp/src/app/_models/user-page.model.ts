import { ChatList } from "./chat-list.model";
import { Chat } from './chat.model';

export class UserPage {
    id: string;
    firstName: string;
    secondName: string;
    usersChats: Chat[];

    constructor(id: string, firstName?: string, secondName?: string, usersChats?: Chat[]) {
        this.id = id;
        this.firstName = firstName;
        this.secondName = secondName;
        this.usersChats = usersChats;
    }
}
