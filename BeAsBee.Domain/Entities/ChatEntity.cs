using System;
using System.Collections.Generic;

namespace BeAsBee.Domain.Entities {
    public class ChatEntity : IEntity<Guid> {
        public Guid Id { set; get; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }
        public List<MessageEntity> Messages { get; set; }
        public List<UserEntity> UserChats { get; set; } // users in this chat
    }
}