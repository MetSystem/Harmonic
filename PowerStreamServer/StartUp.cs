using Autofac;
using Harmonic.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PowerStreamServer
{
    class Startup : IStartup
    {
        public void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<PowerSmartController>().AsSelf();
            builder.RegisterType<StreamService>().As<IStreamService>();
        }
    }
}
