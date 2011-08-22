using System;
using System.Threading;

namespace App2
{
    class Program
    {
        static void Main()
        {
            var logger = new ActionLogger(Console.WriteLine);

            while (true)
            {
                logger.Write("Timestamp: {0} [App2]", DateTime.Now);

                Thread.Sleep(5000);
            }
        }
    }
}
