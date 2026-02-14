namespace EmailService.Common.Models
{
    public class WelcomeUserModel
    {
        public string Username { get; set; } = default!;
        public string TemporaryPassword { get; set; } = default!;
        public string LoginUrl { get; set; } = default!;
    }
}
