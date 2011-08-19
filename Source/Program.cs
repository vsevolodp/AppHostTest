using System;
using System.IO;
using System.Threading;
using Lokad.Cloud.AppHost;
using Lokad.Cloud.AppHost.Framework;

namespace Source
{
    internal class Program
    {
        readonly CancellationTokenSource _cts;
        readonly Host _host;

        static void Main()
        {
            var appHost = new Program();

            appHost.Start();

            Console.CancelKeyPress += (sender, e) => ConsoleCancelKeyPress(appHost);
        }

        static void ConsoleCancelKeyPress(Program appHost)
        {
            appHost.Stop();
        }

        public Program()
        {
            _cts = new CancellationTokenSource();

            var observer = new HostObserverSubject();

            var fileDeploymentReader = new FileDeploymentReader(Directory.GetCurrentDirectory(), "deployment.xml");

            var context = new HostContext(observer, fileDeploymentReader);

            _host = new Host(context);
        }

        public void Start()
        {
            _host.RunSync(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}
