using System;
using Contracts;

namespace App2
{
    class ActionLogger : ILogger
    {
        readonly Action<string, object[]> _action;

        public ActionLogger(Action<string, object[]> action)
        {
            _action = action;
        }

        public void Write(string format, params object[] args)
        {
            _action(format, args);
        }
    }
}
