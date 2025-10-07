// src/MySqlConnector/Diagnostics/SemconvConfig.cs
using System;

namespace MySqlConnector.Diagnostics
{
    public enum SemconvMode { Old, New, Both }

    internal static class SemconvConfig
    {
        public static readonly SemconvMode Mode;

        static SemconvConfig()
        {
            var val = Environment.GetEnvironmentVariable("OTEL_SEMCONV_STABILITY_OPT_IN")?.Trim();
            if (string.IsNullOrEmpty(val))
            {
                Mode = SemconvMode.Old;
                return;
            }

            switch (val.Trim().ToLowerInvariant())
            {
                case "new":
                    Mode = SemconvMode.New;
                    break;
                case "both":
                    Mode = SemconvMode.Both;
                    break;
                case "old":
                    Mode = SemconvMode.Old;
                    break;
                default:
                    // Fallback to old and log somewhere (Trace.WriteLine or logger)
                    Mode = SemconvMode.Old;
                    System.Diagnostics.Debug.WriteLine($"[MySqlConnector] OTEL_SEMCONV_STABILITY_OPT_IN='{val}' not recognized; using 'old'.");
                    break;
            }
        }

        public static bool EmitOld => Mode == SemconvMode.Old || Mode == SemconvMode.Both;
        public static bool EmitNew => Mode == SemconvMode.New || Mode == SemconvMode.Both;
    }
}
