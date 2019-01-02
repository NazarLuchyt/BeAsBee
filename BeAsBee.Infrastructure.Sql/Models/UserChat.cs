using System;

namespace BeAsBee.Infrastructure.Sql.Models {
    public class UserChat {
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }

        //Navigation properties
        public virtual User User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}