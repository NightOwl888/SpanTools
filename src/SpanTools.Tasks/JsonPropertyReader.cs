using System;
using System.IO;
using System.Text;

namespace SpanTools.Tasks
{
    internal sealed class JsonPropertyReader : IDisposable
    {
        private readonly StringBuilder builder = new(capacity: 64);
        private readonly TextReader reader;

        private int current;

        public int CurrentDepth { get; private set; }

        public JsonPropertyReader(Stream stream)
        {
            reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            current = reader.Read();
        }

        public string? ReadPropertyName()
        {
            while (current >= 0)
            {
                switch ((char)current)
                {
                    case '{':
                        CurrentDepth++;
                        Read();
                        break;

                    case '}':
                        CurrentDepth--;
                        Read();
                        break;

                    case '"':
                        {
                            string value = ReadString();

                            SkipWhitespace();

                            if (current == ':')
                            {
                                return value;
                            }

                            break;
                        }

                    default:
                        Read();
                        break;
                }
            }

            return null;
        }

        public void MoveToPropertyValue()
        {
            if (current == ':')
                Read();

            SkipWhitespace();
        }

        public bool ExpectStartObject()
        {
            if (current != '{')
                return false;

            CurrentDepth++;
            Read();

            return true;
        }

        private string ReadString()
        {
            Read();

            builder.Length = 0;

            while (current >= 0)
            {
                char ch = (char)current;

                if (ch == '\\')
                {
                    builder.Append(ch);

                    Read();

                    if (current >= 0)
                    {
                        builder.Append((char)current);
                        Read();
                    }

                    continue;
                }

                if (ch == '"')
                {
                    Read();
                    return builder.ToString();
                }

                builder.Append(ch);
                Read();
            }

            throw new InvalidDataException("Unexpected end of JSON string.");
        }

        private void SkipWhitespace()
        {
            while (current >= 0 && char.IsWhiteSpace((char)current))
            {
                Read();
            }
        }

        private void Read()
        {
            current = reader.Read();
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
