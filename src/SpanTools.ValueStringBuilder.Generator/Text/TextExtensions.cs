// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools.Text
{
    internal static class TextExtensions
    {
        public static LineSplitEnumerator SplitLines(this string text)
        {
            // LineSplitEnumerator is a struct so there is no allocation here
            return new LineSplitEnumerator(text.AsSpan());
        }
    }
}
