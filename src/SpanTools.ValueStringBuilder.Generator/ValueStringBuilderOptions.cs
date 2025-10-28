// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

namespace SpanTools
{
    public sealed record ValueStringBuilderOptions
    {
        public bool AllowUnsafeBlocks { get; set; }
        public string? Namespace { get; set; }
        public bool IncludeMaxLengthTracking { get; set; }
        public bool IncludeAsMemory { get; set; }


        public bool IncludesJ2N
        {
            set
            {
                if (value == true)
                {
                    IncludesJ2N_2_0_Or_Greater = value;
                    IncludesJ2N_2_1_Or_Greater = value;
                    IncludesJ2N_2_2_Or_Greater = value;
                }
            }
        }

        public bool IncludesJ2N_2_0_Or_Greater { get; set; }

        public bool IncludesJ2N_2_1_Or_Greater { get; set; }
        public bool IncludesJ2N_2_2_Or_Greater { get; set; }

        public bool UseJavaStyleFormatting { get; set; }
        public bool IncludeCodepointSupport { get; set; } = true;
    }
}
