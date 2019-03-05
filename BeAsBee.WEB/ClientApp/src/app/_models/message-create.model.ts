export class MessageCreate {
    chatId: string;
    receivedTime: Date;
    messageText: string;

    constructor(chatId: string, receivedTime: Date, messageText: string) {
        this.chatId = chatId;
        this.receivedTime = receivedTime;
        this.messageText = messageText;
    }
}
