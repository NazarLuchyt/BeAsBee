import { Chat } from './chat.model';


export class UserPage {
    id: string;
    firstName: string;
    secondName: string;
    userChats: Chat[];

    constructor(id?: string, firstName?: string, secondName?: string, userChats?: Chat[]) {
        this.id = id;
        this.firstName = firstName;
        this.secondName = secondName;
        this.userChats = userChats;
    }
}
