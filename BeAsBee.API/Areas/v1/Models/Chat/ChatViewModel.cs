using System;
using System.Collections.Generic;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.API.Areas.v1.Models.User;

namespace BeAsBee.API.Areas.v1.Models.Chat {
    public class ChatViewModel {
        public Guid Id { set; get; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public List<UserPageBindingModel> UserChats { get; set; } // users in this chat
    }
}