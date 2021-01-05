using Microsoft.Extensions.Configuration;
using System.IO;

namespace Power
{
    public class Tykd
    {
        private static IConfigurationRoot _config = null;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("tykdconfg.json").Build();
                }

                return _config;
            }
        }
    }
}