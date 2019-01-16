using System;
using System.Collections.Generic;

namespace BeAsBee.Domain.Entities {
    public class UserEntity : IEntity<Guid> {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public List<ChatEntity> Chats { get; set; } // chats where user is an owner
        public List<ChatEntity> UserChats { get; set; } // chats where user is a user
        public List<MessageEntity> Messages { get; set; }
        public Guid Id { get; set; }
    }
}