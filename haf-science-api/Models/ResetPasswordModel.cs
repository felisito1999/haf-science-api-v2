namespace haf_science_api.Models
{
    public class ResetPasswordModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserHash { get; set; }
    }
}
