using System;

namespace BeAsBee.API.Areas.v1.Models.Message {
    public class CreateMessageBindingModel {
        //public Guid Id { get; set; }
        public Guid? ChatId { get; set; }
        public DateTimeOffset ReceivedTime { get; set; }
        public string MessageText { get; set; }
    }
}