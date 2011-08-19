using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Lokad.Cloud.AppHost.Framework;

namespace Source
{
    [Serializable]
    public class FileDeploymentReader : IDeploymentReader
    {
        readonly string _basePath;
        static readonly SHA1 Hash = SHA1.Create();
        readonly string _deploymentFileName;

        public FileDeploymentReader(string basePath, string deploymentFileName)
        {
            _basePath = basePath;
            _deploymentFileName = deploymentFileName;
        }

        public XElement GetHeadIfModified(string knownETag, out string newETag)
        {
            string content;
            Encoding encoding;

            var path = Path.Combine(_basePath, _deploymentFileName);

            if (!File.Exists(path))
            {
                newETag = null;
                return null;
            }

            using (var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
                encoding = reader.CurrentEncoding;
            }

            var hash = Hash.ComputeHash(encoding.GetBytes(content));
            newETag = BitConverter.ToString(hash).Replace("-", "").ToLower();

            if (newETag == (knownETag != null ? knownETag.ToLower() : null))
                return null;

            return XElement.Parse(content);
        }

        public T GetItem<T>(string itemName) where T : class
        {
            var path = Path.Combine(_basePath, itemName);

            if (!File.Exists(path))
                return default(T);

            if (typeof(T).IsAssignableFrom(typeof(XElement)))
            {
                using (var reader = new StreamReader(path))
                {
                    return XElement.Load(reader) as T;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(byte[])))
            {
                return File.ReadAllBytes(path) as T;
            }

            throw new NotSupportedException();
        }
    }
}