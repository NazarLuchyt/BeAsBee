import { Chat } from '../_models/chat.model';

export function setNameForChats(chats: Chat[], id: string): Chat[] {
    chats.map(obj => {
        if (obj.userChats.length <= 2 && !obj.name) {
            const otherUser = obj.userChats.filter(item => item.id !== id)[0];
            obj.name = otherUser.firstName + ' ' + otherUser.secondName;
        }
    });
    return chats;
}

export function setChatName(chat: Chat, id: string): Chat {
    if (chat.userChats.length <= 2 && !chat.name) {
        const otherUser = chat.userChats.filter(item => item.id !== id)[0];
        chat.name = otherUser.firstName + ' ' + otherUser.secondName;
    }
    return chat;
}
