using System.ComponentModel.DataAnnotations;

namespace BeAsBee.API.Areas.v1.Models.User {
    public class CreateUserBindingModel {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }

        [MinLength(4)]
        public string Password { get; set; }
    }
}