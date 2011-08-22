using System;
using System.IO;
using System.Text;
using Contracts;

namespace App1
{
    class StreamLogger : ILogger
    {
        readonly Stream _stream;
        readonly Encoding _encoding;

        public StreamLogger(Stream stream, Encoding encoding)
        {
            _stream = stream;
            _encoding = encoding;
        }

        public void Write(string format, params object[] args)
        {
            var line = string.Format(format + Environment.NewLine, args);

            var bytes = _encoding.GetBytes(line);

            _stream.Write(bytes, 0, bytes.Length);
            _stream.Flush();
        }
    }
}
