using System;

namespace BeAsBee.API.Areas.v1.Models.User {
    public class UserPageBindingModel {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}