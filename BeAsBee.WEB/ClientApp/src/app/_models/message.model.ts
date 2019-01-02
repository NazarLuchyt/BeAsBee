export class Message {
    id: string;
    chatId: string;
    receivedTime: Date;
    messageText: string;
    userName: string;
    userId: string;

    constructor(id: string, chatId: string, receivedTime: Date, messageText: string, userId: string, userName: string) {
        this.id = id;
        this.chatId = chatId;
        this.receivedTime = receivedTime;
        this.messageText = messageText;
        this.userId = userId;
        this.userName = userName;
    }
}