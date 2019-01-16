using System;
using System.ComponentModel.DataAnnotations.Schema;
using BeAsBee.Infrastructure.Sql.Models.Identity;

namespace BeAsBee.Infrastructure.Sql.Models {
    public class Message {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? ChatId { get; set; }
        public virtual Chat Chat { get; set; } //Navigation property

        public DateTimeOffset ReceivedTime { get; set; }
        public string MessageText { get; set; }
       
        public Guid? UserId { get; set; }
        public virtual User User { get; set; } //Navigation property
    }
}