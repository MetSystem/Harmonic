using Microsoft.Extensions.Configuration;
using System.IO;

namespace Power.WebStream
{
    public class WebManager
    {
        private static IConfigurationRoot _config = null;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("webconfg.json").Build();
                }

                return _config;
            }
        }
    }
}