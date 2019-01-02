using System;

namespace BeAsBee.API.Areas.v1.Models.User {
    public class UserViewModel {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        //public List<ChatEntity> Chats { get; set; } // chats where user is an owner
        //public List<ChatEntity> UserChats { get; set; } // chats where user is a user
        //public List<MessageEntity> Messages { get; set; }
    }
}