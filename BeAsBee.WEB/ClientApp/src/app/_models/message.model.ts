export class Message {
    id: string;
    chatId: string;
    receivedTime: Date;
    messageText: string;
    userName: string;
    userId: string;

    constructor(id: string, messageText: string, chatId?: string, receivedTime?: Date,  userId?: string, userName?: string) {
        this.id = id;
        this.chatId = chatId;
        this.receivedTime = receivedTime;
        this.messageText = messageText;
        this.userId = userId;
        this.userName = userName;
    }
}