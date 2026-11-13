using PennForgeTech.Core.Exceptions;

namespace PennForgeTech.Core.Configuration
{
    public static class RequiredSetting
    {
        public const string MissingCode = "Configuration.Missing";

        public static string Require(string? value, string settingName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new PennForgeException(MissingCode, $"Configuration value '{settingName}' is required.");
            }

            return value;
        }
    }
}
