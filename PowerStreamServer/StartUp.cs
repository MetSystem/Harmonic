using Autofac;
using Harmonic.Hosting;
using Microsoft.Extensions.Configuration;
using Tianyi;

namespace PowerStreamServer
{
    internal class Startup : IStartup
    {
        public void ConfigureServices(ContainerBuilder builder)
        {
            var powerOptions = Power.Configuration.GetSection("Server").Get<PowerOptions>();
            var tianyiOptions = Power.Configuration.GetSection("Tykd.Config").Get<TianyiOptions>();
            builder.RegisterType<PowerSmartController>().AsSelf();
            if (powerOptions.StreamType == "Defualt")
            {
                builder.RegisterType<TykdStreamService>().As<IStreamService>();
            }
            if (powerOptions.StreamType == "Tykd")
            {
                builder.RegisterType<StreamService>().As<IStreamService>();
            }
            builder.RegisterInstance<PowerOptions>(powerOptions);
            builder.RegisterInstance<TianyiService>(new TianyiService(tianyiOptions.AppKey, tianyiOptions.AppSecret, tianyiOptions.Account));
        }
    }
}