using System;

namespace BeAsBee.API.Areas.v1.Models.Chat {
    public class ChatListBindingModel {
        public Guid Id { set; get; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }
    }
}