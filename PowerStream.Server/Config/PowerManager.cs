using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.IO;

namespace PowerStream.Server
{
    public class PowerManager
    {
        public static ConcurrentBag<StreamConnection> FFmpegProcessList = new ConcurrentBag<StreamConnection>();

        private static IConfigurationRoot _config = null;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                }

                return _config;
            }
        }
    }
}