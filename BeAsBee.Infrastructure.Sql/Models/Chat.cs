using System;
using System.Collections.Generic;

namespace BeAsBee.Infrastructure.Sql.Models {
    public class Chat {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid? UserId { get; set; }

        //Navigation properties
        public virtual User User { get; set; }
        public virtual List<Message> Messages { get; set; } = new List<Message>();
        public virtual List<UserChat> UserChats { get; set; } = new List<UserChat>(); // users in this chat
    }
}