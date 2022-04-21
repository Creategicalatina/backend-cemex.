namespace back_end_cemex.Helpers.Email.ForgotPassword
{
    public class UserForgotPassword
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TokenURL { get; set; }
        public string Token { get; set; }
    }
}
