using System.Diagnostics;

namespace PennForgeTech.Core.Diagnostics
{
    public static class PennForgeDiagnostics
    {
        public const string SourceName = "PennForgeTech.Core";

        public static readonly ActivitySource ActivitySource = new ActivitySource(SourceName);
    }
}
