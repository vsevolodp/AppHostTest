using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Contracts;
using Lokad.Cloud.AppHost.Framework;

namespace App1
{
    class EntryPoint : IApplicationEntryPoint
    {
        readonly ILogger _logger;

        public EntryPoint()
        {
            var writer = File.AppendText("app1_log.txt");
            _logger = new StreamLogger(writer.BaseStream, writer.Encoding);
        }

        public void Run(XElement settings, IDeploymentReader deploymentReader, IApplicationEnvironment environment, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.Write("Timestamp: {0} [App1]", DateTime.Now);

                cancellationToken.WaitHandle.WaitOne(5000);
            }
        }

        public void ApplyChangedSettings(XElement settings)
        {
            _logger.Write("SettingsChanged: {0} [App1]", settings.ToString());
        }
    }
}
