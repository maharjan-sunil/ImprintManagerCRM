
namespace EmailService.Options
{
    public class SmtpSettingOptions
    {
        public const string SectionName = "SmtpSettings";

        public required string Host { get; init; }
        public int Port { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string DefaultFrom { get; init; }
        public required string DisplayName { get; init; }
        public bool UseSSL { get; init; } = true;
    }
}
