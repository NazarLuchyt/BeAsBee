export class MessageCreate {
    chatId: string;
    receivedTime: Date;
    messageText: string;
    userName: string;
    userId: string;
   
    constructor(chatId: string, receivedTime: Date, messageText: string, userId: string, userName: string) {
        this.chatId = chatId;
        this.receivedTime = receivedTime;
        this.messageText = messageText;
        this.userId = userId;
        this.userName = userName;
    }
}
