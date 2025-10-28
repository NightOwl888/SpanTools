// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace SpanTools
{
    public sealed class CodeBuilder
    {
        private readonly IndentedTextWriter _writer;

        public CodeBuilder(StringBuilder sb, string indent = "    ")
            => _writer = new IndentedTextWriter(new StringWriter(sb), indent);

        public CodeBuilder WriteLine() { _writer.WriteLine(); return this; }
        public CodeBuilder WriteLine(string line) { _writer.WriteLine(line); return this; }
        public CodeBuilder IndentBlock(Action body)
        {
            _writer.Indent++;
            body();
            _writer.Indent--;
            return this;
        }

        public CodeBuilder WriteLineIf(bool condition, string line)
        {
            if (condition)
                _writer.WriteLine(line);
            return this;
        }

        public CodeBuilder BlockIf(bool condition, Action body)
        {
            if (condition)
                body();
            return this;
        }

        public CodeBuilder SectionIf(bool condition, string header, Action body)
        {
            if (!condition) return this;
            WriteLine(header);
            WriteLine("{");
            _writer.Indent++;
            body();
            _writer.Indent--;
            WriteLine("}");
            return this;
        }

        public override string ToString() => ((StringWriter)_writer.InnerWriter).ToString();
    }
}
