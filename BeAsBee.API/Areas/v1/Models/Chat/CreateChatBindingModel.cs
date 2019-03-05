using System;
using System.Collections.Generic;
using BeAsBee.Domain.Entities;

namespace BeAsBee.API.Areas.v1.Models.Chat {
    public class CreateChatBindingModel {
        public string Name { get; set; }
        public List<UserEntity> UserChats { get; set; } // users in this chat
    }
}