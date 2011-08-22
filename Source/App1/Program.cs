using System;
using System.Threading;

namespace App1
{
    class Program
    {
        static void Main()
        {
            var logger = new StreamLogger(Console.OpenStandardOutput(), Console.OutputEncoding);

            while (true)
            {
                logger.Write("Timestamp: {0} [App1]", DateTime.Now);

                Thread.Sleep(1000);
            }
        }
    }
}
