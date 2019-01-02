using System;

namespace BeAsBee.Domain.Entities {
    public class MessageEntity : IEntity<Guid> {
        public Guid Id { get; set; }
        public Guid? ChatId { get; set; }
        public DateTimeOffset ReceivedTime { get; set; }
        public string MessageText { get; set; }
        public string UserName { get; set; }
        public Guid? UserId { get; set; }
    }
}