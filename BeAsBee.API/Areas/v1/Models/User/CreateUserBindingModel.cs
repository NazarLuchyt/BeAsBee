namespace BeAsBee.API.Areas.v1.Models.User {
    public class CreateUserBindingModel {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}