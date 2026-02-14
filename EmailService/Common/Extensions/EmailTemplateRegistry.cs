using Shared.Common.Constants;
using EmailService.Common.Models;

namespace EmailService.Common.Extensions
{
    public static class EmailTemplateRegistry
    {
        private static readonly Dictionary<string, Type> _templateModels = new()
    {
        { EmailTemplates.WelcomeUser, typeof(WelcomeUserModel) },
        { EmailTemplates.PasswordReset, typeof(PasswordResetModel) },
        // Add new templates here
    };

        public static Type? GetModelType(string templateName)
        {
            _templateModels.TryGetValue(templateName, out var type);
            return type;
        }
    }
}
