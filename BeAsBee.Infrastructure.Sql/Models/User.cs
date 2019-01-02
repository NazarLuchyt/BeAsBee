using System;
using System.Collections.Generic;

namespace BeAsBee.Infrastructure.Sql.Models {
    public class User {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //Navigation properties
        public virtual List<Chat> Chats { get; set; } = new List<Chat>(); // chats where user is an owner

        public virtual List<UserChat> UserChats { get; set; } = new List<UserChat>(); // chats where user is a user

        public virtual List<Message> Messages { get; set; } = new List<Message>();
    }
}